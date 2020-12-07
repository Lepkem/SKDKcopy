﻿using Microsoft.AspNetCore.Mvc;using System;using System.Collections.Generic;using System.Linq;using System.Threading.Tasks;using FluentValidation.Results;using Microsoft.Extensions.Configuration;using Stexchange.Data;using Stexchange.Data.Models;using Stexchange.Data.Builders;using Stexchange.Data.Helpers;using Stexchange.Data.Validation;using static System.String;using Microsoft.AspNetCore.Http;
using System.IO;

namespace Stexchange.Controllers{    public class AdvertisementController : Controller    {        public AdvertisementController(Database db, IConfiguration config)        {            _database = db;            _config = config;        }        private Database _database { get; }        private IConfiguration _config { get; }        public IActionResult PostAdvertisement()        {            return View();        }        public IActionResult Posted()        {            return View("Posted");        }

        public async Task OnPostUploadAsync(List<IFormFile> files, Listing finishedListing)
        {
            //creates memorystream for each image
            foreach (IFormFile file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Upload the file if less than 5 MB
                    if (memoryStream.Length < 4997152)
                    {
                        var imagefile = new ImageData()
                        {
                            Image = memoryStream.ToArray(),
                            Listing = finishedListing,
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
        }        [HttpPost]        public async Task<IActionResult> PostAdvertisement(List<IFormFile> files, string title, string description,             string name_nl, uint quantity, string plant_type, string give_away, string with_pot, string name_lt="",
            string light = "", string water = "", string ph = "", string indigenous = "", string nutrients="")        {            try            {                if (ModelState.IsValid)                {                    ListingBuilder listingBuilder = new ListingBuilder();                    Listing finishedListing;                    ListingValidator listingVal = new ListingValidator();                    WithPotFilterValidator potVal = new WithPotFilterValidator();                    GiveAwayFilterValidator giveVal = new GiveAwayFilterValidator();                    PlantTypeFilterValidator typeVal = new PlantTypeFilterValidator();                    List<Filter> mandatoryFilters = new List<Filter>();

                    ValidationResult hasTypeFilter = typeVal.Validate(new Filter { Value = plant_type });                    ValidationResult hasPotFilter = potVal.Validate(new Filter{ Value = with_pot });                    ValidationResult hasGiveFilter = giveVal.Validate(new Filter{ Value = give_away });                    ValidationResult hasValProps = listingVal.Validate(new Listing                                                                            {                                                                                Description = description,                                                                                Title = title,                                                                                NameNl = name_nl,                                                                                Quantity = quantity                                                                            });

                    if (hasValProps.IsValid && hasPotFilter.IsValid && hasGiveFilter.IsValid)                    {
                        listingBuilder.SetProperty("Title", StandardMessages.CapitalizeFirst(title).Trim())
                                      .SetProperty("Description", StandardMessages.CapitalizeFirst(description).Trim())
                                      .SetProperty("NameNl", StandardMessages.CapitalizeFirst(name_nl).Trim())
                                      .SetProperty("Quantity", quantity)                                      .SetProperty("Visible", true)                                      .SetProperty("Renewed", false)                                      .SetProperty("UserId", 1); // TODO: USER_ID

                        // adding validated required filters to list
                        mandatoryFilters.Add(new Filter{ Value = with_pot });                        mandatoryFilters.Add(new Filter{ Value = give_away });
                        mandatoryFilters.Add(new Filter{ Value = plant_type });

                        // non-required properties
                        List<Filter> validatedFilters = FilterListValidator(mandatoryFilters, ph, water, indigenous, light, nutrients);                        if (!IsNullOrEmpty(name_lt)) listingBuilder.SetProperty("NameLatin", StandardMessages.CapitalizeFirst(name_lt).Trim());                        finishedListing = listingBuilder.Complete();                        List<FilterListing> filterListings = MakeFilterListing(validatedFilters, finishedListing);

                        // ensures that the listing is inserted before the tables who need this FK
                        await _database.AddAsync(finishedListing);

                        // Insert byte[] image into database
                        await OnPostUploadAsync(files, finishedListing);
                        // loops through filterlist to add each advertisementfilter                        await _database.AddRangeAsync(filterListings);

                        //passing data to the view
                        TempData["Title"] = finishedListing.Title;                        TempData.Keep("Title");                        await _database.SaveChangesAsync();                        return RedirectToAction("Posted");                    }                    //todo: passing error messages to the view                    if (!hasPotFilter.IsValid)                    {                         foreach (ValidationFailure error in hasPotFilter.Errors)                        {                            //todo do something with this                            //error.ErrorMessage;                        }                    }                    if (!hasGiveFilter.IsValid)                    {                        foreach (ValidationFailure error in hasGiveFilter.Errors)                        {                            //todo do something with this                            //error.ErrorMessage;                        }                    }                    if (!hasValProps.IsValid)                    {                        foreach (ValidationFailure error in hasValProps.Errors)                        {                            //todo do something with this                            //error.ErrorMessage;                        }                    }                }            }            catch (Exception e)            {                Console.WriteLine(e.ToString());                ViewBag.Error = "Error: " + e.ToString();            }            return View();        }

        /// <summary>        /// adds each filter in filterlist and the advertisement listing to list of filterlistings.        /// </summary>        /// <param name="filters"></param>        /// <param name="finishedListing"></param>        /// <returns></returns>        private List<FilterListing> MakeFilterListing(List<Filter> filters, Listing finishedListing)        {            List<FilterListing> filterListings = new List<FilterListing>();            foreach (Filter fil in filters)            {
                // adding new filterlisting instance to filterListings
                filterListings.Add(new FilterListing { Listing = finishedListing, Value = fil.Value });            }            return filterListings;        }

        /// <summary>
        /// validates filter values and adds these to the filterlist
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="ph"></param>
        /// <param name="water"></param>
        /// <param name="indigenous"></param>
        /// <param name="light"></param>
        /// <param name="nutrients"></param>
        /// <returns></returns>        private List<Filter> FilterListValidator(List<Filter> filters, string ph, string water, string indigenous, string light, string nutrients)        {            PhFilterValidator phVal = new PhFilterValidator();            WaterFilterValidator waterVal = new WaterFilterValidator();            LightFilterValidator lightVal = new LightFilterValidator();            IndigenousFilterValidator indiVal = new IndigenousFilterValidator();            Filter phFilter = new Filter { Value = ph };            Filter waterFilter = new Filter { Value = water };            Filter lightFilter = new Filter { Value = light };            Filter indigenousFilter = new Filter { Value = indigenous };            Filter nutrientsFilter = new Filter { Value = nutrients };            if (phVal.Validate(phFilter).IsValid) filters.Add(phFilter);            if (waterVal.Validate(waterFilter).IsValid) filters.Add(waterFilter);             if (lightVal.Validate(lightFilter).IsValid) filters.Add(lightFilter);            if (indiVal.Validate(indigenousFilter).IsValid) filters.Add(indigenousFilter);
            if (indiVal.Validate(nutrientsFilter).IsValid) filters.Add(nutrientsFilter);
            return filters;        }    }}