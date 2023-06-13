using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{
    private new string name;
    private Currency currency;
    [SerializeField] private int prosperity; // How rich is a country [0, Max_pros(5 for now)]
    [SerializeField] private double balance;
    [SerializeField] private double gdp = 0;
    private int exports = 0; // negative if imports more.
    private Dictionary<Service, double> importTax = new Dictionary<Service, double>(); // [0-0.3]
    private Dictionary<Service, double> exportTax = new Dictionary<Service, double>(); // [(-0.1)-0.1]



    /// <summary>
    /// This function initializes the country's name, currency, prosperity, and balance
    /// </summary>
    /// <param name="name">The name of the country.</param>
    /// <param name="Currency">The currency that the country uses.</param>
    /// <param name="prosperity">This is a number between 1 and 10 that represents how prosperous the
    /// country is.</param>
    public void init(string name, Currency currency, int prosperity)
    {
        this.Name = name;
        this.Currency = currency;
        this.Prosperity = prosperity;
        this.Balance = this.Prosperity * Random.Range(100f, 10000f);
        this.currency.Supply += this.Balance;
        this.currency.Demand += this.balance;
    }

    /// <summary>
    /// This function adds 10% of the exports to the supply of the currency
    /// </summary>
    public void addSupplyToCurrency()
    {
        if (exports > 0)
        {
            this.currency.Supply += exports / 10;
        }
    }


    /// <summary>
    /// It sets the import and export tax values for each service
    /// </summary>
    public void setTaxes()
    {
        // Set import tax values
        for (int i = 0; i < Main.SERVICE_COUNT; i++)
        {
            ImportTax[Main.Services[i]] = Random.Range(0f, 0.3f);
        }

        // Set export tax values
        for (int i = 0; i < Main.SERVICE_COUNT; i++)
        {
            ExportTax[Main.Services[i]] = Random.Range(-0.1f, 0.1f);
        }
    }

    // Getter setter
    public string Name { get => name; set => name = value; }
    public Currency Currency { get => currency; set => currency = value; }
    public int Prosperity { get => prosperity; set => prosperity = value; }
    public Dictionary<Service, double> ImportTax { get => importTax; set => importTax = value; }
    public Dictionary<Service, double> ExportTax { get => exportTax; set => exportTax = value; }
    public double Balance { get => balance; set => balance = value; }
    public double Gdp { get => gdp; set => gdp = value; }
    public int Exports { get => exports; set => exports = value; }

    ///
}
