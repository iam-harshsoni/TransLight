using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Models;

namespace TransLight.DataAccess.Data;

public partial class TransLightContext : DbContext
{
    public TransLightContext()
    {
    }

    public TransLightContext(DbContextOptions<TransLightContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__banks__3214EC0751932BEB");

            entity.ToTable("banks");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("cities");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.District)
                .HasMaxLength(255)
                .HasColumnName("district");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Pincode).HasColumnName("pincode");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cities_states");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("companies");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AccountContact)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("account_contact");
            entity.Property(e => e.AccountEmail)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("account_email");
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("account_no");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Bank)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("bank");
            entity.Property(e => e.BlDraftEmail)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("bl_draft_email");
            entity.Property(e => e.Branch)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("branch");
            entity.Property(e => e.ChaLicenseNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("cha_license_no");
            entity.Property(e => e.ChaNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("cha_no");
            entity.Property(e => e.CinNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("cin_no");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Contact)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("contact");
            entity.Property(e => e.EinvoiceAuthToken)
                .HasMaxLength(50)
                .HasColumnName("einvoice_auth_token");
            entity.Property(e => e.EinvoicePassword)
                .HasMaxLength(50)
                .HasColumnName("einvoice_password");
            entity.Property(e => e.EinvoiceTokenExpiry)
                .HasColumnType("datetime")
                .HasColumnName("einvoice_token_expiry");
            entity.Property(e => e.EinvoiceUsername)
                .HasMaxLength(50)
                .HasColumnName("einvoice_username");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("email");
            entity.Property(e => e.GstNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("gst_no");
            entity.Property(e => e.Guid)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .IsFixedLength()
                .HasColumnName("guid");
            entity.Property(e => e.IfscCode)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("ifsc_code");
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("logo");
            entity.Property(e => e.MsmeNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("msme_no");
            entity.Property(e => e.MtoRegiNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("mto_regi_no");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PanNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("pan_no");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("remarks");
            entity.Property(e => e.Signature)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("signature");
            entity.Property(e => e.TallyName)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("tally_name");
            entity.Property(e => e.TanNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("tan_no");
            entity.Property(e => e.TermsConditions)
                .HasColumnType("text")
                .HasColumnName("terms_conditions");
            entity.Property(e => e.ThemeColor)
                .HasMaxLength(10)
                .HasColumnName("theme_color");
            entity.Property(e => e.UsdAccountNo)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("usd_account_no");
            entity.Property(e => e.UsdBank)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("usd_bank");
            entity.Property(e => e.UsdBranch)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("usd_branch");
            entity.Property(e => e.UsdIfscCode)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("usd_ifsc_code");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .IsFixedLength()
                .HasColumnName("uuid");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("website");

            entity.HasOne(d => d.City).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_companies_cities");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__countrie__3214EC076906DBE6");

            entity.ToTable("countries");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("currencies");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("states");

            entity.HasIndex(e => e.CountryId, "IX_states_countries");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.Gst)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gst");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UnionTerritory).HasColumnName("union_territory");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_states_countries");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("units");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
