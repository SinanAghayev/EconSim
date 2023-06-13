using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] private string currencyName;
    [SerializeField] Dictionary<Currency, double> exchangeRate = new Dictionary<Currency, double>(); // c1_c2_exc * c2_c1_exc = 1
    // 1 this.curr = x other curr
    [SerializeField] private double demand = 1;
    [SerializeField] private double supply = 1;
    [SerializeField] private double value = 1;

    /// <summary>
    /// This function initializes the currency name
    /// </summary>
    /// <param name="name">The name of the currency.</param>
    public void init(string name)
    {
        this.CurrencyName = name;
    }

    /// <summary>
    /// This function takes the demand and supply of a product and divides them to get the value of the
    /// product
    /// </summary>
    public void adjustValue()
    {
        //if (this.demand > this.supply)
        //   this.supply += (this.supply - this.demand) / this.supply;
        this.Value = this.demand / this.supply;
    }

    /// GETTER SETTERS
    public string CurrencyName { get => currencyName; set => currencyName = value; }
    public Dictionary<Currency, double> ExchangeRate { get => exchangeRate; set => exchangeRate = value; }
    public double Demand { get => demand; set => demand = value; }
    public double Supply { get => supply; set => supply = value; }
    public double Value { get => value; set => this.value = value; }
}
