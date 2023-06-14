using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    private string p_name;
    private int age; // groups, 0 child 1 teen 2 adult 3 elder
    private int gender; // 0 w, 1 m
    private Country country;
    private Currency currency;
    [SerializeField] private double balance;
    private double prev_balance;
    private double saveUrge; // [0,0.8]
    private ArrayList services = new ArrayList();
    private int boughtServices = 0;
    // If larger than 0.8? must buy; lower than saveUrge, wont buy
    // Age bias 0.2, gender bias 0.2, personal bias 0.6
    [SerializeField] private Dictionary<Service, double> prefService = new Dictionary<Service, double>();

    /// <summary>
    /// This function initializes the person's name, age, gender, country, currency, balance, and save
    /// urge
    /// </summary>
    /// <param name="name">the name of the person</param>
    /// <param name="age">groups, 0 = child, 1 = teen, 2 = adult, 3 = elder</param>
    /// <param name="gender">0 = female, 1 = male</param>
    /// <param name="Country">The country the person is from.</param>
    public void init(string name, int age, int gender, Country country)
    {
        this.Name = name;
        this.Age = age;
        this.Gender = gender;
        this.Country = country;
        this.currency = country.Currency;

        this.Balance = (this.Age + 1) * this.Country.Prosperity * Random.Range(1f, 100f);
        this.prev_balance = this.balance;
        this.currency.Supply += this.balance;
        this.currency.Demand += this.balance;
        this.SaveUrge = Random.Range(0, 0.8f);
    }

    // TODO UPDATE FUNCTION (INCLUDE TRYBUYING, CHANGE SAVEURGE( HAS MORE MONEY = LESS SAVEURGE ) ETC.)

    public void personNext()
    {
        tryBuying();
        setSaveUrge();
        prev_balance = balance;
    }

    /// <summary>
    /// The function sets the preference of a person for a service
    /// </summary>
    public void setPreferences()
    {
        // Initialize preference for every service
        foreach (Service service in Main.Services)
        {
            double servicePref = 0.2 * service.AgePref[this.Age]
                            + 0.2 * service.GenderPref[this.Gender]
                            + 0.3 * Random.Range(0f, 1f)
                            - 0.2 * (service.Price / this.balance)
                            + 0.3 * (service.BasePrice - service.Price) / service.Price;
            // Person cannot buy own service
            if (service.Seller == this)
                servicePref = -1;
            if (PrefService.ContainsKey(service))
            {
                PrefService[service] = servicePref;
            }
            else
            {
                PrefService.Add(service, servicePref);
            }
            if (servicePref > saveUrge)
            {
                service.Demand++;
            }
            if (servicePref < saveUrge && service.Demand > 1)
            {
                service.Demand--;
            }
        }
    }

    void setSaveUrge()
    {
        // Some people spend their money even if they don't have, some doesn't spend even if they have.
        if (Random.Range(0, 1f) < 0.1)
        {
            this.saveUrge *= (balance / prev_balance);
        }

        this.saveUrge = (this.saveUrge < 0.8) ? this.saveUrge : 0.8;
    }

    /// <summary>
    /// If the urge to buy a service is greater than the urge to save, and the service is available, buy
    /// it
    /// </summary>
    public void tryBuying()
    {
        ArrayList bought = new ArrayList();
        foreach (Service service in this.prefService.Keys)
        {
            // There is a little randomness so people will sometimes chose not to buy even if they can.
            if (prefService[service] > this.saveUrge && service.Supply > 1 && Random.Range(0f, 1f) > 0.5)
            {
                buy(service);
                bought.Add(service);
            }
        }
        foreach (Service s in bought)
        {
            this.prefService[s] *= this.prefService[s];
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
        if (this.balance < priceInLocal * (1 + this.country.ImportTax[service]))
            return;
        if (this.country != service.OriginCountry)
            this.country.Gdp -= priceInLocal;

        this.balance -= priceInLocal * (1 + this.country.ImportTax[service]);
        service.Seller.balance += service.Price * (1 - service.OriginCountry.ExportTax[service]);

        this.country.Balance += priceInLocal * this.country.ImportTax[service];
        service.OriginCountry.Balance += service.Price * service.OriginCountry.ExportTax[service];

        this.currency.Demand -= priceInLocal;
        service.Currency.Demand += service.Price;

        this.country.Exports--;
        service.OriginCountry.Exports++;

        this.BoughtServices++;
        service.OriginCountry.Gdp += service.Price;
        service.Supply--;

        Service.Services_bought++;
    }


    /// <summary>
    /// If the object that collided with this object is a person, then move the person away from this
    /// object by half the distance between them
    /// </summary>
    /// <param name="Collision">The collision that happened</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Person"))
        {
            // We dont include y in our calculations because we dont want to change y
            float xDiff = Mathf.Abs(other.gameObject.transform.position.x - gameObject.transform.position.x); // x difference
            float zDiff = Mathf.Abs(other.gameObject.transform.position.z - gameObject.transform.position.z); // z difference
            float diff = Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff); // Distance without y
            Vector3 dir = other.gameObject.transform.position - gameObject.transform.position; // Direction from this object to other
            dir.y = 0; // We dont want to change y
            dir /= diff; // Normalize the vector so it is unit vector
            other.gameObject.transform.position += dir * 0.5f; // Change the position of other object in the direction we found
        }
    }


    /// GETTER SETTERS
    public string Name { get => name; set => name = value; }
    public int Age { get => age; set => age = value; }
    public int Gender { get => gender; set => gender = value; }
    public Country Country { get => country; set => country = value; }
    public double Balance { get => balance; set => balance = value; }
    public double SaveUrge { get => saveUrge; set => saveUrge = value; }
    public ArrayList Services { get => services; set => services = value; }
    public Dictionary<Service, double> PrefService { get => prefService; set => prefService = value; }
    public int BoughtServices { get => boughtServices; set => boughtServices = value; }
    public double Prev_balance { get => prev_balance; set => prev_balance = value; }
}
