using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wörterbuch.AppData;
using Wörterbuch.AppData.Entities;
using Wörterbuch.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using HtmlAgilityPack;
using System.Net;
using System.Text;
using System.Globalization;


namespace Wörterbuch.Controllers;

public class HomeController : Controller
{
	[HttpGet("/List")]
	public IActionResult List([FromServices] AppDbContext appDbContext)
	{
		var list = appDbContext.Words.OrderBy(w => w.Word)
		.AsEnumerable() // Fetches data first
		.Select(e =>
		{
			// e.VerbConjugation = e?.VerbConjugation?.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
			return e;
		})
		.ToList();
		return View(list);
	}

	[HttpGet("/")]
	[HttpGet("/Edit/{id}")]
	public IActionResult Index([FromServices] AppDbContext appDbContext, [FromServices] IMapper mapper,  [FromRoute(Name = "id")] string? wordId)
	{
		var viewModel = new WordEntityVM();

		if (!string.IsNullOrEmpty(wordId))
		{
			var entity = appDbContext.Words.FirstOrDefault(w => w.Id == Convert.ToInt64(wordId));
			mapper.Map(entity, viewModel);
		}

		viewModel.TypeOptions = Enum.GetValues(typeof(WordType))
		.Cast<WordType>()
		.Select(g => new SelectListItem
		{
			Value = g.ToString(),
			Text = g.ToString()
		});

		return View(viewModel);
	}

	[HttpPost("/Save")]
	public IActionResult Save([FromServices] AppDbContext appDbContext, [FromServices] IMapper mapper, WordEntityVM ve)
	{
		WordEntity we = new WordEntity();

		mapper.Map(ve, we);

/* 		we.Word = ve.Word;
		we.Type = ve.Type!.Value;
		we.Pronunciation = ve.Pronunciation;
		we.Plural = ve.Plural;
		we.Gender = ve.Gender;
		we.VerbConjugation = ve.VerbConjugation;
		we.English = ve.English;
		we.Turkish = ve.Turkish;
		we.Sample = ve.Sample;
		we.Detail = ve.Detail; */

		if(we.Id > 0) {
			appDbContext.Words.Update(we);
			appDbContext.SaveChanges();
		}
		else
		{
			appDbContext.Words.Add(we);
			appDbContext.SaveChanges();			
		}

		return RedirectToAction("Index", new { Id = we.Id });
	}

	[HttpPost("/Lookup")]
	[ValidateAntiForgeryToken]	
	public IActionResult Lookup([FromServices] AppDbContext appDbContext, [FromBody] LookupVM model)
	{
		LookupVM lookupVM = new LookupVM();

		try
		{
			if (model == null || string.IsNullOrEmpty(model.Word))
			{
				lookupVM.Message = "Word is null";
				return BadRequest(lookupVM);
			}

			// HtmlDocument htmlDocument = new HtmlWeb().Load($"https://www.verbformen.com/?w={model.Word}");
			// var vStckKrz = htmlDocument.GetElementbyId("vStckKrz");
			// var vGrnd = vStckKrz.SelectSingleNode(".//*[contains(@class, 'vGrnd')]");
			// lookupVM.Found = vGrnd.InnerText;
			// lookupVM.IsNoun = new[] { "der ", "die ", "das " }.Any(prefix => lookupVM.Found.StartsWith(prefix));
			// var r1Zeile = vStckKrz.SelectSingleNode(".//*[contains(@class, 'r1Zeile')]");
			// lookupVM.Meaning = NormalizeString(r1Zeile.InnerText);

			var w = appDbContext.Words.FirstOrDefault(w => w.Word == model.Word);

			if(w is null)
			{

			}
			else
			{
				lookupVM.Found = true;
				lookupVM.Link = $"/Edit/{w.Id}";				
			}
		}
		catch (Exception ex)
		{
			lookupVM.Exception = ex?.Message;
		}

		return Json(lookupVM);
	}

	private string NormalizeString(string input)
	{
		input = WebUtility.HtmlDecode(input);
		return new string(input
			.Normalize(NormalizationForm.FormD) // Decomposes diacritics
			.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) // Removes diacritics
			.Where(c => !char.IsControl(c)) // Removes control characters
			.ToArray());
	}
}


