using System;

namespace Wörterbuch.ViewModels;

public class LookupVM
{
	public string? Word { get; set; }	
	public bool Found { get; set; }
	public string? Link { get; set; }
	public string? Message { get; set; }
	public string? Exception { get; set; }
}
