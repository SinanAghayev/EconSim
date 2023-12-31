using System.Collections.Generic;
using UnityEngine;

public class Service : MonoBehaviour
{
    public enum TYPE
    {
        GOVERNMENT = 0,
        REGULAR = 1,
        ALL = 2,
    }

    private static int services_bought = 0;
    private string p_name;
    [SerializeField] private double basePrice;
    [SerializeField] private double price;
    private double previous_month_price;
    private int demand = 1;
    private int supply = 1;
    private int new_supply = 0; // How much the supply will change
    private TYPE type_of_supply;
    // TODO new type of service that governments buy
    private Currency currency;
    private Country originCountry;
    private Person seller;

    private Dictionary<int, double> agePref = new Dictionary<int, double>()
    {
        { 0, 0 },
        { 1, 0 },
        { 2, 0 },
        { 3, 0 }
    };
    private Dictionary<int, double> genderPref = new Dictionary<int, double>()
    {
        { 0, 0 },
        { 1, 0 }
    };

    /// <summary>
    /// This function initializes the service with a name, base price, country of origin, and seller. It
    /// also initializes the age and gender preferences of the service
    /// </summary>
    /// <param name="name">The name of the service</param>
    /// <param name="basePrice">The base price of the service.</param>
    /// <param name="Country">The country where the service is being sold.</param>
    /// <param name="Person">The person who is selling the service</param>
    public void init(string name, double basePrice, int initialSupply, Person seller, int t)
    {
        this.Name = name;
        this.BasePrice = basePrice; // Maybe limit
        this.supply = initialSupply;
        this.OriginCountry = seller.Country;
        this.originCountry.Services.Add(this);
        this.seller = seller;
        this.type_of_supply = (TYPE)t;

        this.price = this.basePrice;
        this.previous_month_price = price;
        this.Currency = this.OriginCountry.Currency;
        this.Currency.Demand += this.price;
        this.new_supply = Random.Range(1, 20);
        seller.Services.Add(this);
        // Init age pref values
        for (int i = 0; i < 4; i++)
        {
            AgePref[i] = Random.Range(0f, 1f);
        }
        // Init gender pref values
        for (int i = 0; i < 2; i++)
        {
            GenderPref[i] = Random.Range(0f, 1f);
        }
    }

    /// <summary>
    /// This function adds the new supply to the supply
    /// </summary>
    public void addSupply()
    {
        if (this.supply * 2 < this.demand && this.seller.Balance > this.new_supply * this.price)
        {
            invest();
        }
        if (this.supply < this.demand)
        {
            this.supply += this.new_supply;
        }
        // TODO change this function (it doesnt work logically; when no one buys it, supply continues to increase IT SHOULDN'T)
    }

    public void invest()
    {
        this.seller.Balance -= this.new_supply * this.price;
        this.New_supply *= 11 / 10;
        // TODO COUNTRY INVEST
    }

    /// <summary>
    /// This function adjusts the price of the item based on the demand and supply of the item
    /// </summary>
    public void adjustPrice()
    {
        previous_month_price = price;
        this.price = (double)Demand / (double)Supply * BasePrice;
    }

    /// GETTER SETTERS
    public string Name { get => name; set => name = value; }
    public double Price { get => price; set => price = value; }
    public Currency Currency { get => currency; set => currency = value; }
    public Dictionary<int, double> AgePref { get => agePref; set => agePref = value; }
    public Dictionary<int, double> GenderPref { get => genderPref; set => genderPref = value; }
    public Person Seller { get => seller; set => seller = value; }
    public Country OriginCountry { get => originCountry; set => originCountry = value; }
    public double BasePrice { get => basePrice; set => basePrice = value; }
    public int Demand { get => demand; set => demand = value; }
    public int Supply { get => supply; set => supply = value; }
    public int New_supply { get => new_supply; set => new_supply = value; }
    public static int Services_bought { get => services_bought; set => services_bought = value; }
    public double Previous_month_price { get => previous_month_price; set => previous_month_price = value; }
    public TYPE Type_of_supply { get => type_of_supply; set => type_of_supply = value; }
}
