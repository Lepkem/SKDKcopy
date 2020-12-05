using Microsoft.AspNetCore.Mvc;using System;using System.Collections.Generic;using System.Linq;using System.Threading.Tasks;using FluentValidation.Results;using Microsoft.Extensions.Configuration;using Stexchange.Data;using Stexchange.Data.Models;using Stexchange.Data.Builders;using Stexchange.Data.Helpers;using Stexchange.Data.Validation;using static System.String;using Microsoft.AspNetCore.Http;using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Web.Helpers;
using System.IO.Compression;

namespace Stexchange.Controllers{    public class AdvertisementController : Controller    {        public AdvertisementController(Database db, IConfiguration config)        {            _database = db;            _config = config;        }        private Database _database { get; }        private IConfiguration _config { get; }        public IActionResult PostAdvertisement()        {            return View();        }        public IActionResult Posted()        {            return View("Posted");        }

        public async Task OnPostUploadAsync(List<IFormFile> files)
        {
            //creates memorystream for each image
            foreach (IFormFile file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        var imagefile = new ImageData()
                        {
                            Image = memoryStream.ToArray(),
                            //Listing = finishedListing,
                        };

                        _database.Add(imagefile);
                        await _database.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
        }        [HttpPost]        public async Task<IActionResult> PostAdvertisement(List<IFormFile> files, string title, string description,             string name_nl, int quantity, string plant_type, string give_away, string with_pot, string name_lt="",
            string light = "", string water = "", string ph = "", string indigenous = "")        {            //todo: def val planttype and withpot            try            {                if (ModelState.IsValid)                {                    ListingBuilder listingBuilder = new ListingBuilder();                    Listing finishedListing;                    ListingValidator listingVal = new ListingValidator();                    WithPotFilterValidator potVal = new WithPotFilterValidator();                    GiveAwayFilterValidator giveVal = new GiveAwayFilterValidator();                    PlantTypeFilterValidator typeVal = new PlantTypeFilterValidator();                    List<Filter> mandatoryFilters = new List<Filter>();

                    Console.WriteLine($"{files}");
                    await OnPostUploadAsync(files);                    Console.WriteLine($"image:");                     //bool hasImages = images.Any();
                    // Console.WriteLine($"images: {hasImages}");
                    ValidationResult hasTypeFilter = typeVal.Validate(new Filter { Value = plant_type });                    ValidationResult hasPotFilter = potVal.Validate(new Filter{Value = with_pot});                    ValidationResult hasGiveFilter = giveVal.Validate(new Filter{Value = give_away});                    ValidationResult hasValProps = listingVal.Validate(new Listing()                                                                            {                                                                                Description = description,                                                                                Title = title,                                                                                NameNl = name_nl,                                                                                Quantity = quantity                                                                            });

                    //using fluent validator to required properties
                    Console.WriteLine($"valid: {hasValProps.IsValid}, {hasPotFilter.IsValid}, {hasGiveFilter.IsValid}");
                    //if (hasImages && hasValProps.IsValid && hasPotFilter.IsValid && hasGiveFilter.IsValid)
                    if (hasValProps.IsValid && hasPotFilter.IsValid && hasGiveFilter.IsValid)                    {                        DateTime created = DateTime.Now;
                        listingBuilder.SetProperty("Title", StandardMessages.CapitalizeFirst(title).Trim())
                                      .SetProperty("Description", StandardMessages.CapitalizeFirst(description).Trim())
                                      .SetProperty("NameNl", StandardMessages.CapitalizeFirst(name_nl).Trim())
                                      .SetProperty("Quantity", quantity)
                                      .SetProperty("CreatedAt", created)                                      .SetProperty("Visible", true)                                      .SetProperty("Renewed", false)                                      .SetProperty("LastModified", created)                                      .SetProperty("UserId", 1);


                        //adding validated required filters to list
                        mandatoryFilters.Add(new Filter{Value = with_pot});                        mandatoryFilters.Add(new Filter{Value = give_away});

                        //now the non-required properties
                        List<Filter> validatedFilters = FilterListValidator(mandatoryFilters, ph, water, indigenous, light);                        if (!IsNullOrEmpty(name_lt)) listingBuilder.SetProperty("NameLt", StandardMessages.CapitalizeFirst(name_lt).Trim());                                                finishedListing = listingBuilder.Complete();                        List<FilterListing> filterListings = MakeFilterListing(validatedFilters, finishedListing);
                        //List<ImageData> imageDataListings = MakeImageDataListing(images, finishedListing);

                        Console.WriteLine(finishedListing.Description);
                        //ensures that the listing is made before the filters who need this FK
                        await _database.AddAsync(finishedListing);                        Console.WriteLine("add");                        await _database.AddRangeAsync(validatedFilters);                        await _database.AddRangeAsync(filterListings);
                        //await _database.AddRangeAsync(imageDataListings);
                        Console.WriteLine("Addrange");

                        //passing data to the view
                        TempData["Title"] = finishedListing.Title;                        TempData.Keep("Title");                        await _database.SaveChangesAsync();                        Console.WriteLine("SaveChange");                        return RedirectToAction("Posted");                    }                    //todo: passing error messages to the view                    if (!hasPotFilter.IsValid)                    {                         foreach (ValidationFailure error in hasPotFilter.Errors)                        {                            //todo do something with this                            //error.ErrorMessage;                        }                    }                    if (!hasGiveFilter.IsValid)                    {                        foreach (ValidationFailure error in hasGiveFilter.Errors)                        {                            //todo do something with this                            //error.ErrorMessage;                        }                    }                    if (!hasValProps.IsValid)                    {                        foreach (ValidationFailure error in hasValProps.Errors)                        {                            //todo do something with this                            //error.ErrorMessage;                        }                    }                    //if (!images.Any())                    //{                    //    //todo do something with this                    //    StandardMessages.RequiredField("pictures");                    //}                }            }            catch (Exception e)            {                Console.WriteLine(e.ToString());                ViewBag.Error = "Error: " + e.ToString();            }            return View();        }



        /// <summary>        /// adding the properties of filterlisting to link the listing to their filters        /// </summary>        /// <param name="filters"></param>        /// <param name="finishedListing"></param>        /// <returns></returns>        private List<FilterListing> MakeFilterListing(List<Filter> filters, Listing finishedListing)        {            List<FilterListing> filterListings = new List<FilterListing>();            foreach (Filter fil in filters)            {
                //todo add value in 
                //adding new filterlisting instance to filterListings
                
                filterListings.Add(new FilterListing { Listing = finishedListing, Value = fil.Value });            }            return filterListings;        }        //private List<ImageData> MakeImageDataListing(List<byte[]> images, Listing finishedlisListing)        //{        //    List<ImageData> ImageDataListing = new List<ImageData>();        //    foreach (byte[] image in images)        //    {        //        ImageDataListing.Add(new ImageData(finishedlisListing, image));        //    }        //    return ImageDataListing;        //}

        /// <summary>
        /// adds validated props to filterlist
        /// </summary>
        /// <param name="filts"></param>
        /// <param name="ph"></param>
        /// <param name="water"></param>
        /// <param name="plant_type"></param>
        /// <param name="indigenous"></param>
        /// <param name="light"></param>
        /// <returns></returns>        private List<Filter> FilterListValidator(List<Filter> filters, string ph, string water, string indigenous, string light)        {            PhFilterValidator phVal = new PhFilterValidator();            WaterFilterValidator waterVal = new WaterFilterValidator();            LightFilterValidator lightVal = new LightFilterValidator();            IndigenousFilterValidator indiVal = new IndigenousFilterValidator();            Filter phFilter = new Filter { Value = ph };            Filter waterFilter = new Filter { Value = water };            Filter lightFilter = new Filter { Value = light };            Filter indigenousFilter = new Filter { Value = indigenous };            if (phVal.Validate(phFilter).IsValid) filters.Add(phFilter);            if (waterVal.Validate(waterFilter).IsValid) filters.Add(waterFilter);             if (lightVal.Validate(lightFilter).IsValid) filters.Add(lightFilter);            if (indiVal.Validate(indigenousFilter).IsValid) filters.Add(indigenousFilter);

            Console.WriteLine($"FilterList: {filters}");            return filters;        }                    }}