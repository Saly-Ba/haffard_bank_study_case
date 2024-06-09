using Microsoft.EntityFrameworkCore;
using HaffardBankApi.Models;

namespace HaffardBankApi.Data;

public class HaffardDbContext : DbContext {

    public DbSet<ClientModel> Client {get; set;}
    public DbSet<CardModel> Card {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=localhost;port=8889;user=haffard;password=1234;database=haffard_bank_db";

        var serverVersion = new MySqlServerVersion(new Version(10, 4, 21));

        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
}