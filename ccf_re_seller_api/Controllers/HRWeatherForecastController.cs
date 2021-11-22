﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ccf_re_seller_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HRWeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public HRWeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("hr/")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}