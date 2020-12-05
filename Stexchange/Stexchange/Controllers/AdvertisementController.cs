﻿using Microsoft.AspNetCore.Mvc;

namespace Stexchange.Controllers
            string light = "", string water = "", string ph = "", string indigenous = "")
                    Console.WriteLine(quantity);
                    // Console.WriteLine($"images: {hasImages}");
                    ValidationResult hasTypeFilter = typeVal.Validate(new Filter { Value = plant_type });

                    //using fluent validator to required properties
                    Console.WriteLine($"valid: {hasValProps.IsValid}, {hasPotFilter.IsValid}, {hasGiveFilter.IsValid}");
                    //if (hasImages && hasValProps.IsValid && hasPotFilter.IsValid && hasGiveFilter.IsValid)
                    if (hasValProps.IsValid && hasPotFilter.IsValid && hasGiveFilter.IsValid)
                        listingBuilder.SetProperty("Title", StandardMessages.CapitalizeFirst(title).Trim())
                                      .SetProperty("Description", StandardMessages.CapitalizeFirst(description).Trim())
                                      .SetProperty("NameNl", StandardMessages.CapitalizeFirst(name_nl).Trim())
                                      .SetProperty("Quantity", quantity)
                                      .SetProperty("CreatedAt", created)


                        //adding validated required filters to list
                        mandatoryFilters.Add(new Filter{Value = with_pot});

                        //now the non-required properties
                        List<Filter> validatedFilters = FilterListValidator(mandatoryFilters, ph, water, indigenous, light);
                        //List<ImageData> imageDataListings = MakeImageDataListing(images, finishedListing);

                        Console.WriteLine(finishedListing.Description);
                        //ensures that the listing is made before the filters who need this FK
                        var ad_test = new Listing()
                        {
                            Title = "Prachtig Plantje",
                            Description = "dengrens, teksten die uit slechts één teken of klank bestaan. In vele gevallen bestaat een tekst echter uit een aantal welgeordende woorden en zinnen, die min of meer coherentie en cohesie vertonen. Aan de bovengrens is er dan bijvoorbeeld sprake van een romantekst, of een lang gesprek of gewoon een dialoog tussen twee personen.",
                            NameNl = "Bosbes",
                            NameLatin = "Verbes",
                            Quantity = 2,
                            CreatedAt = created,
                            Visible = true,
                            Renewed = false,
                            LastModified = created,
                            UserId = 1
                        };
                        await _database.AddAsync(finishedListing);
                        //await _database.AddRangeAsync(imageDataListings);
                        Console.WriteLine("Addrange");
                        //passing data to the view
                        TempData["Title"] = finishedListing.Title;



        /// <summary>
                //todo add value in 
                //adding new filterlisting instance to filterListings
                
                filterListings.Add(new FilterListing{ ListingId = 5, Value = filters.First().Value});

        /// <summary>
        /// adds validated props to filterlist
        /// </summary>
        /// <param name="filts"></param>
        /// <param name="ph"></param>
        /// <param name="water"></param>
        /// <param name="plant_type"></param>
        /// <param name="indigenous"></param>
        /// <param name="light"></param>
        /// <returns></returns>