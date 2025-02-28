using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wörterbuch.AppData.Entities;

public class WordEntity
{	
	public int Id { get; set; }
	public string Word { get; set; } = null!;
	public WordType Type { get; set; }	
	public string? Pronunciation { get; set; } = null!;
	public string? Plural { get; set; } = null!;
	public string? Gender { get; set; } = null!;
	public string? VerbConjugation { get; set; } = null!;	
	public string? English { get; set; }
	public string? Turkish { get; set; }
	public string? Sample { get; set; }
	public string? Detail { get; set; }
}

public class WordEntityConfiguration : IEntityTypeConfiguration<WordEntity>
{
	public void Configure(EntityTypeBuilder<WordEntity> builder)
	{
		// builder.HasKey(b => b.Word); // Composite Key: .HasKey(e => new { e.Key1, e.Key2 });
		// builder.HasKey(b => new { b.Word, b.Type });
		builder.Property(b => b.Word).IsRequired();
		builder.HasIndex(b => new { b.Word, b.Type }).IsUnique(true);
	}
}
