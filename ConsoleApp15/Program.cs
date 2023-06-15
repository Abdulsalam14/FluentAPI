using Microsoft.EntityFrameworkCore;


using (var db = new AcademyContext())
{
}
public class Group
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }
    public int Year { get; set; }
}


public class Chair
{
    public int Id { get; set; }
    public decimal Financing { get; set; }
    public string? Name { get; set; }
}

public class Faculty
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class Teacher
{
    public int Id { get; set; }
    public DateTime EmploynmentDate { get; set; }
    public string? Name { get; set; }
    public decimal Premium { get; set; }
    public decimal WageRate { get; set; }
    public string? Surname { get; set; }

}


public class AcademyContext : DbContext
{

    public AcademyContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Academy;Integrated Security=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Groups");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired();
            entity.Property(t => t.Name).HasMaxLength(10);
            entity.HasIndex(t => t.Name).IsUnique();
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Groups_Name", "LEN(Name) > 0"));
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Groups_Rating", "Rating BETWEEN 0 and 5"));
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Groups_Year", "Year BETWEEN 1 and 5"));
        });
        modelBuilder.Entity<Chair>(entity =>
        {
            entity.ToTable("Chairs");
            entity.HasKey(c=> c.Id);
            entity.Property(c => c.Financing).HasColumnType("money");
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Chairs_Financing", "Financing >= 0"));
            entity.Property(c => c.Financing).HasDefaultValue(0);
            entity.Property(c => c.Name).HasMaxLength(100);
            entity.Property(c => c.Name).IsRequired();
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Chairs_Name", "LEN(Name) > 0"));
            entity.HasAlternateKey(entity => entity.Name);
        });
        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.ToTable("Faculties");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Name).HasMaxLength(100);
            entity.Property(c => c.Name).IsRequired();
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Faculties_Name", "LEN(Name) > 0"));
            entity.HasIndex(t => t.Name).IsUnique();
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teachers");
            entity.Property(t => t.EmploynmentDate).HasColumnType("date");
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Teachers_EmploynmentDate", "EmploynmentDate > '01,01,1990'"));
            entity.Property(t => t.Name).IsRequired();
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Teachers_Name", "LEN(Name) > 0"));
            entity.Property(t => t.Premium).HasColumnType("money");
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Teachers_Premium", "Premium >= 0"));
            entity.Property(t => t.Premium).HasDefaultValue(0);
            entity.Property(t => t.WageRate).HasColumnType("money");
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Teachers_WageRate", "WageRate > 0"));
            entity.Property(t => t.Surname).IsRequired();
            entity.ToTable(entity => entity.HasCheckConstraint("CK_Teachers_Surname", "LEN(Surname) > 0"));


        });
    }
}