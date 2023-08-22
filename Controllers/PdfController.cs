

using IronPdf.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace MapboxPdfDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class PdfController : ControllerBase
{
	private readonly ILogger<PdfController> _logger;

	public PdfController(ILogger<PdfController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	[Route("live-map")]
	public async Task<ActionResult> GetLiveMap()
	{
		_logger.LogInformation("Starting IronPDF conversion");
		var url = new Uri("https://docs.mapbox.com/mapbox-gl-js/example/simple-map/", UriKind.Absolute);
		var renderer = CreateChromePdfRenderer();
		var result = await renderer.RenderUrlAsPdfAsync(url);
		result.SaveAs("output-live-map.pdf");
		_logger.LogInformation("Finished IronPDF conversion");
		return Ok();
	}

	[HttpGet]
	[Route("static-map")]
	public async Task<ActionResult> GetStaticMap()
	{
		_logger.LogInformation("Starting IronPDF conversion");
		var renderer = CreateChromePdfRenderer();
		var result = await renderer.RenderHtmlAsPdfAsync(GetStaticMapHtml(true));
		result.SaveAs("output-static-map.pdf");
		_logger.LogInformation("Finished IronPDF conversion");
		return Ok();
	}
	
	[HttpGet]
	[Route("static-text")]
	public async Task<ActionResult> GetStaticHello()
	{
		_logger.LogInformation("Starting IronPDF conversion");
		var renderer = CreateChromePdfRenderer();
		var result = await renderer.RenderHtmlAsPdfAsync(GetStaticMapHtml(false));
		result.SaveAs("output-static-hello.pdf");
		_logger.LogInformation("Finished IronPDF conversion");
		return Ok();
	}

	private static ChromePdfRenderer CreateChromePdfRenderer()
	{

		var renderingOptions = new ChromePdfRenderOptions
		{
			CreatePdfFormsFromHtml = false,
			MarginBottom = 20,
			MarginLeft = 15,
			MarginRight = 15,
			MarginTop = 15,
			Timeout = 120,
			CssMediaType = PdfCssMediaType.Print,
		};

		var renderDelay = TimeSpan.FromSeconds(10);
		renderingOptions.WaitFor.RenderDelay((int)renderDelay.TotalMilliseconds);

		var renderer = new ChromePdfRenderer { RenderingOptions = renderingOptions };
		return renderer;
	}

	private static string GetStaticMapHtml(bool includeMapScript)
	{
		string mapScriptHtml = @"
<script>
	mapboxgl.accessToken = 'pk.eyJ1IjoiaHAtYWVyb3F1YWwiLCJhIjoiY2xnNDgxMHpnMGY1ODNtcjMwN2liazF2eSJ9.BYYIXim7eFcQDLQrPyV4Tg';
const map = new mapboxgl.Map({
container: 'map', // container ID
// Choose from Mapbox's core styles, or make your own style with Mapbox Studio
style: 'mapbox://styles/mapbox/streets-v12', // style URL
center: [-74.5, 40], // starting position [lng, lat]
zoom: 9 // starting zoom
});
</script>
";
        
	var scriptHtml = includeMapScript ? mapScriptHtml : string.Empty;	
		
	return @"
<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8"">
<title>Display a map on a webpage</title>
<meta name=""viewport"" content=""initial-scale=1,maximum-scale=1,user-scalable=no"">
<link href=""https://api.mapbox.com/mapbox-gl-js/v2.15.0/mapbox-gl.css"" rel=""stylesheet"">
<script src=""https://api.mapbox.com/mapbox-gl-js/v2.15.0/mapbox-gl.js""></script>
<style>
body { margin: 0; padding: 0; }
#map { position: absolute; top: 0; bottom: 0; width: 100%; }
</style>
</head>
<body>
<h1 id=""title"">Hello, World!</h1>
<div id=""map""></div>
" + scriptHtml + @"
<script>
const title = document.getElementById('title');
title.textContent = ""Map Demo"";
</script>
</body>
</html>
";
	}

}
	
	
