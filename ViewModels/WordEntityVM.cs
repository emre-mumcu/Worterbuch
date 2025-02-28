using Microsoft.AspNetCore.Mvc.Rendering;
using Wörterbuch.AppData.Entities;

namespace Wörterbuch.ViewModels;

public class WordEntityVM : WordEntity
{
	// WordEntity.Type is NOT nullable.
	// To make it nullable, use new keyword and hide the base member to make select elements first item as 'Select a value' string.
	public new WordType? Type { get; set; }
	public IEnumerable<SelectListItem>? TypeOptions { get; set; }	
}
