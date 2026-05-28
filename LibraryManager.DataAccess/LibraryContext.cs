using LibraryManager.Models.Entities;
using LibraryManager.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.DataAccess;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Reader> Readers => Set<Reader>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(x => x.AuthorId);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.CategoryId);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(300);
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(x => x.BookId);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Isbn).HasMaxLength(30).IsRequired();
            entity.HasIndex(x => x.Isbn).IsUnique();

            entity.HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(x => x.ReaderId);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30);
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(x => x.LoanId);
            entity.Property(x => x.Status).HasConversion<int>().HasDefaultValue(LoanStatus.Active);

            entity.HasOne(x => x.Book)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Reader)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
