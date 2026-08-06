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

    public virtual DbSet<CompanySite> CompanySites { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductRawMaterial> ProductRawMaterials { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

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
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Contact)
                .HasMaxLength(255)
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
            entity.Property(e => e.Pincode).HasColumnName("pincode");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("remarks");
            entity.Property(e => e.Signature)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("signature");
            entity.Property(e => e.Stamp)
                .HasMaxLength(50)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("stamp");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .HasColumnName("state");
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
        });

        modelBuilder.Entity<CompanySite>(entity =>
        {
            entity.ToTable("company_sites");

            entity.HasIndex(e => e.CompanyId, "IX_company_sites_companies");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("contact");
            entity.Property(e => e.EinvoiceUsername)
                .HasMaxLength(50)
                .HasColumnName("einvoice_username");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("email");
            entity.Property(e => e.EwayPassword)
                .HasMaxLength(50)
                .HasColumnName("eway_password");
            entity.Property(e => e.EwayUsername)
                .HasMaxLength(50)
                .HasColumnName("eway_username");
            entity.Property(e => e.GstNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("gst_no");
            entity.Property(e => e.LutNo)
                .HasMaxLength(30)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("lut_no");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Pincode).HasColumnName("pincode");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanySites)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_company_sites_companies");
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

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.HasIndex(e => e.CategoryId, "IX_products_categories");

            entity.HasIndex(e => e.UnitId, "IX_products_units");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Gst)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("gst");
            entity.Property(e => e.Hsn)
                .HasMaxLength(255)
                .HasColumnName("hsn");
            entity.Property(e => e.Make)
                .HasMaxLength(255)
                .HasColumnName("make");
            entity.Property(e => e.Msl)
                .HasDefaultValue(0)
                .HasColumnName("msl");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Pack)
                .HasMaxLength(255)
                .HasColumnName("pack");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("rate");
            entity.Property(e => e.TallyNamePurchase)
                .HasMaxLength(255)
                .HasColumnName("tally_name_purchase");
            entity.Property(e => e.TallyNameSales)
                .HasMaxLength(255)
                .HasColumnName("tally_name_sales");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_products_categories");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_products_units");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("product_categories");

            entity.HasIndex(e => e.Id, "IX_product_categories_id");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProductRawMaterial>(entity =>
        {
            entity.ToTable("product_raw_materials");

            entity.HasIndex(e => e.ProductId, "IX_product_raw_materials_product_id");

            entity.HasIndex(e => e.RawMaterialId, "IX_product_raw_materials_raw_material_id");

            entity.HasIndex(e => e.UnitId, "IX_product_raw_materials_unit_id");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Qty)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("qty");
            entity.Property(e => e.RawMaterialId).HasColumnName("raw_material_id");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductRawMaterialProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_raw_materials_product");

            entity.HasOne(d => d.RawMaterial).WithMany(p => p.ProductRawMaterialRawMaterials)
                .HasForeignKey(d => d.RawMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_raw_materials_raw_material");

            entity.HasOne(d => d.Unit).WithMany(p => p.ProductRawMaterials)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_product_raw_materials_unit");
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

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transactions");

            entity.HasIndex(e => e.CompanyId, "IX_transactions_companies");

            entity.HasIndex(e => e.CompanySiteId, "IX_transactions_company_sites");

            entity.HasIndex(e => e.CurrencyId, "IX_transactions_currencies");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.BasicAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("basic_amt");
            entity.Property(e => e.Cancel).HasColumnName("cancel");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CompanySiteId).HasColumnName("company_site_id");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeliveryType).HasColumnName("delivery_type");
            entity.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("exchange_rate");
            entity.Property(e => e.GstAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("gst_amt");
            entity.Property(e => e.Id2Format)
                .HasMaxLength(255)
                .HasColumnName("id2_format");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.PartySiteId).HasColumnName("party_site_id");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasColumnName("remarks");
            entity.Property(e => e.RoundOffAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("round_off_amt");
            entity.Property(e => e.TotalAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("total_amt");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Company).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transactions_companies");

            entity.HasOne(d => d.CompanySite).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CompanySiteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transactions_company_sites");

            entity.HasOne(d => d.Currency).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transactions_currencies");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.ToTable("transaction_details");

            entity.HasIndex(e => e.ProductId, "IX_transaction_details_products");

            entity.HasIndex(e => e.TransactionId, "IX_transaction_details_transactions");

            entity.HasIndex(e => e.UnitId, "IX_transaction_details_units");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.BasicAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("basic_amt");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.GstAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("gst_amt");
            entity.Property(e => e.GstPer)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("gst_per");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("rate");
            entity.Property(e => e.SrNo)
                .HasMaxLength(255)
                .HasColumnName("sr_no");
            entity.Property(e => e.TotalAmt)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("total_amt");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.Vertical)
                .HasMaxLength(255)
                .HasColumnName("vertical");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_details_products");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_details_transactions");

            entity.HasOne(d => d.Unit).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transaction_details_units");
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
