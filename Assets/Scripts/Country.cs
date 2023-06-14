using System.Collections;
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

    private ArrayList services = new ArrayList();
    private double inflation = 0;

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


    public void countryNext()
    {
        tryBuying();
    }

    /// <summary>
    /// If the urge to buy a service is greater than the urge to save, and the service is available, buy
    /// it
    /// </summary>
    public void tryBuying()
    {
        foreach (Service service in Main.Services)
        {
            // There is a little randomness so governments will sometimes chose not to buy even if they can.
            if (service.Type_of_supply != Service.TYPE.REGULAR && service.Supply > 1 && Random.Range(0f, 10f) > 0.1)
            {
                buy(service);
            }
        }

    }

    /// <summary>
    /// A person buys a service, and the price of the service is adjusted
    /// </summary>
    /// <param name="Service">The service that is being bought</param>
    /// <returns>
    /// Nothing
    /// </returns>
    public void buy(Service service)
    {
        double priceInLocal = service.Price * service.Currency.ExchangeRate[this.currency];
        if (priceInLocal > balance)
            return;
        if (this != service.OriginCountry)
            this.Gdp -= priceInLocal;

        service.OriginCountry.Gdp += service.Price;

        this.balance -= priceInLocal;

        service.Seller.Balance += service.Price * (1 - service.OriginCountry.ExportTax[service]);
        service.OriginCountry.Balance += service.Price * service.OriginCountry.ExportTax[service];

        this.currency.Demand -= priceInLocal;
        service.Currency.Demand += service.Price;

        service.Supply--;
        Service.Services_bought++;
    }


    // TODO this function seems stupid check it later
    /// <summary>
    /// This function adds 10% of the exports to the supply of the currency
    /// </summary>
    public void addSupplyToCurrency()
    {
        currency.adjustValue();
        if (currency.Value > 1)
        {
            this.balance += currency.Supply / 10;
            this.currency.Supply += currency.Supply / 10;
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
    public ArrayList Services { get => services; set => services = value; }
    public double Inflation { get => inflation; set => inflation = value; }

    ///
}
