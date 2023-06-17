using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;

public class Main : MonoBehaviour
{
    public static readonly int COUNTRY_COUNT = 20; // Also CURRENCY_COUNT
    public static readonly int SERVICE_COUNT = 1500;
    public static readonly int PEOPLE_COUNT = 500;
    public static readonly int CEIL_PRICE = 500;
    public static readonly int MAX_PROSPERITY = 20;
    public static readonly int COUNTRY_SIZE = 2;
    public static readonly int MAX_DAY = 500;
    public static readonly int BLOCK_SIZE = 500;

    private static List<string> toWriteCountries = new List<string>();
    private static List<string> toWriteCurrencies = new List<string>();
    private static List<string> toWritePeople = new List<string>();
    private static List<string> toWriteServices = new List<string>();

    private static List<Country> countries = new List<Country>();
    private static List<Currency> currencies = new List<Currency>();
    private static List<Service> services = new List<Service>();
    private static List<Person> people = new List<Person>();

    public Button button;
    int day = 0;

    /// <summary>
    /// It initializes the currencies, countries, people, and services, and then sets all the
    /// preferences and taxes
    /// </summary>
    void Start()
    {
        button.onClick.AddListener(next);

        writeSettings();
        initCurrencies();
        initCountries();
        initPeople();
        initServices();
        setAllPreferences();
        setTaxes();
    }

    /// <summary>
    /// If the day is less than the maximum day, then increment the day and print it. If the day
    /// is equal to the maximum day, then write the data to a file and increment the day
    /// </summary>
    private void Update()
    {
        if (day < MAX_DAY + 1)
        {
            if (day % BLOCK_SIZE == 0)
            {
                if (day > 0)
                {
                    writeToFiles(true);
                }
                else
                {
                    writeToFiles(false);
                }
            }
            next();
        }
    }

    /// <summary>
    /// This function is called every time the user clicks the "Next" button. It increments the day,
    /// sets the exchange rates, sets the preferences of all the people, has everyone buy, and adds
    /// supplies to the services
    /// </summary>
    void next()
    {
        appendToCountryStrings(day);
        appendToCurrencyStrings(day);
        appendToServiceStrings(day);
        appendToPeopleStrings(day);
        day++;
        setExchangeRates();
        peopleCountryActions();
        // Do these every month
        if (day % 30 == 0)
        {
            print(day);
            calculateInflation();
            addCurrencySupplies();
            addServiceSupplies();
            adjustPrices();
            setAllPreferences();
        }
    }

    private void calculateInflation()
    {
        foreach (Country country in countries)
        {
            double currCPI = 0, prevCPI = 0;
            foreach (Service s in country.Services)
            {
                currCPI += s.Price;
                prevCPI += s.Previous_month_price;
            }
            country.Inflation = ((currCPI - prevCPI) / prevCPI) * 100;
        }
    }

    public void adjustPrices()
    {
        foreach (Service service in services)
        {
            if (Random.Range(0f, 1f) < 0.1)
            {
                service.adjustPrice();
            }

        }
    }

    /// <summary>
    /// This function loops through the list of services and calls the addSupply() function for each
    /// service
    /// </summary>
    public void addServiceSupplies()
    {
        foreach (Service service in Services)
        {
            service.addSupply();
        }
    }

    public void addCurrencySupplies()
    {
        foreach (Country country in countries)
        {
            country.addSupplyToCurrency();
        }
    }

    /// <summary>
    /// "Everyone tries to buy stuff, and if they can't, they die." (AI wrote it, i will keep it because its hilarious)
    /// </summary>
    public void peopleCountryActions()
    {
        foreach (Person person in People)
        {
            person.personNext();
        }
        foreach (Country country in countries)
        {
            country.countryNext();
        }
    }

    /// <summary>
    /// For each currency, set the exchange rate for each other currency
    /// </summary>
    void setExchangeRates()
    {
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            for (int j = 0; j < COUNTRY_COUNT; j++)
            {
                double rate = Currencies[i].Value / Currencies[j].Value;
                Currencies[i].ExchangeRate[Currencies[j]] = rate;
            }
        }

        for (int i = 0; i < COUNTRY_COUNT; i++)
        {

            for (int j = 0; j < COUNTRY_COUNT; j++)
            {
                Currency c1 = Currencies[i];
                Currency c2 = Currencies[j];
            }
        }
    }


    /// <summary>
    /// For each person in the list of people, set their preferences
    /// </summary>
    void setAllPreferences()
    {
        foreach (Person person in People)
        {
            person.setPreferences();
        }
    }

    /// <summary>
    /// It creates a list of currencies, and then sets the exchange rate of each currency to 1 for every
    /// other currency
    /// </summary>
    void initCurrencies()
    {
        // Init currencies
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            GameObject currency = new GameObject("Currency " + i);
            Currency c = currency.AddComponent<Currency>();
            c.init("Currency " + i);
            Currencies.Add(c);
        }
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            for (int j = 0; j < COUNTRY_COUNT; j++)
            {
                Currencies[i].ExchangeRate.Add(Currencies[j], 1);
            }
        }
    }

    // TODO DOC
    void initCountries()
    {
        // Init countries
        float x = 0, z = 0;
        int sign = 1, changeSign = 1;
        bool xORz = false;
        int changeXORZ = 1, changeSwitch = 0; // xORz : true: x, false: z
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            int pros = UnityEngine.Random.Range(1, MAX_PROSPERITY); // Get random prosperity for every country
            GameObject country = GameObject.CreatePrimitive(PrimitiveType.Plane); // Create plane
            MeshRenderer meshRenderer = country.GetComponent<MeshRenderer>(); // Get mesh renderer
            Material material = new Material(Shader.Find("Standard")); // Create material
            material.color = UnityEngine.Random.ColorHSV(); // Assign a random color
            meshRenderer.material = material; // Add material to the object
            country.name = "Country " + i;
            Country c = country.AddComponent<Country>();
            c.init("Country " + i, Currencies[i], pros);
            Countries.Add(c);
            country.transform.localScale = new Vector3(COUNTRY_SIZE, country.transform.localScale.y, COUNTRY_SIZE); // Change scale
            country.transform.position = new Vector3(x, country.transform.position.y, z);// Choose location


            // These are done to get the spiral shape of countries
            // change the sign when needed
            if (changeSign * (changeSign + 1) == i)
            {
                changeSign++;
                sign *= -1;
            }
            // Change the dimension when needed
            if ((changeXORZ * changeXORZ == i) || ((changeXORZ - 1) * changeXORZ == i))
            {
                xORz = !xORz;
                changeSwitch++;
            }
            // Change changeXORZ to 0 when it is 2.
            if (changeSwitch == 2)
            {
                changeXORZ++;
                changeSwitch = 0;
            }
            // Change x or z by the amount
            if (xORz)
            {
                x += 10 * COUNTRY_SIZE * sign;
            }
            else
            {
                z += 10 * COUNTRY_SIZE * sign;
            }
        }
    }

    /// <summary>
    /// It creates services, each with a random price, and assigns them to a random person
    /// </summary>
    void initServices()
    {
        // Init Services
        for (int i = 0; i < SERVICE_COUNT; i++)
        {
            double price = UnityEngine.Random.Range(1f, CEIL_PRICE);
            int initialSupply = UnityEngine.Random.Range(1, 100);

            GameObject service = new GameObject("Service " + i);
            Service s = service.AddComponent<Service>();

            int rnd = UnityEngine.Random.Range(0, Main.PEOPLE_COUNT);
            s.init("Service " + i, price, initialSupply, People[rnd], UnityEngine.Random.Range(0, 3));
            Services.Add(s);
        }
    }

    /// <summary>
    /// It creates a random number of people, gives them a random age, gender and country, and then
    /// places them in a random position in their country
    /// </summary>
    void initPeople()
    {
        // Init people
        for (int i = 0; i < PEOPLE_COUNT; i++)
        {
            int age = UnityEngine.Random.Range(0, 4); // Give random age to person
            int gender = UnityEngine.Random.Range(0, 2); // Give random gender to person
            int country = UnityEngine.Random.Range(0, COUNTRY_COUNT); // Give random country to person

            GameObject person = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create person as capsule
            person.name = "Person " + i; // Give a name to person

            Person p = person.AddComponent<Person>(); // Add "object" to the gameobject
            p.init("Person " + i, age, gender, (Country)Countries[country]); // Initialize person with given info
            People.Add(p); // Add person to the list of people

            MeshRenderer meshRenderer = person.GetComponent<MeshRenderer>(); // Create mesh renderer
            Material material = new Material(Shader.Find("Standard")); // Create material so we can change color
            material.color = p.Country.GetComponent<MeshRenderer>().material.color; // Change color depending on country to make a good visualisation
            meshRenderer.material = material;

            Vector3 countryPos = p.Country.gameObject.transform.position;
            float height = 0.5f + 0.25f * age; // Initialize height using age
                                               // Give it a random position
            person.transform.position = new Vector3(UnityEngine.Random.Range(countryPos.x - COUNTRY_SIZE * 4.5f, countryPos.x + COUNTRY_SIZE * 4.5f),
                                                     height,
                                                    UnityEngine.Random.Range(countryPos.z - COUNTRY_SIZE * 4.5f, countryPos.z + COUNTRY_SIZE * 4.5f));
            // Change scale using height variable
            person.transform.localScale = new Vector3(person.transform.lossyScale.x, height, person.transform.lossyScale.x);

            // To prevent them from overlapping
            Rigidbody rb = person.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;

            /* For gender
            GameObject hat = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Create hat gameobject as sphere
            MeshRenderer hatMeshRenderer = hat.GetComponent<MeshRenderer>(); // Create renderer
            Material hatMaterial = new Material(Shader.Find("Standard")); // Create material
            if (gender == 0)
                hatMaterial.color = Color.magenta;
            else
                hatMaterial.color = Color.blue;
            hatMeshRenderer.material = hatMaterial;
            hat.transform.parent = person.transform;
            hat.transform.localPosition = new Vector3(0, 0.6f, 0);
            */
        }
    }

    /// <summary>
    /// This function sets the taxes for each country
    /// </summary>
    void setTaxes()
    {
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            Countries[i].setTaxes();
        }
    }

    /// Writing data to files

    /// <summary>
    /// It writes the data to the files
    /// </summary>
    void writeToFiles(bool mode)
    {
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            string filePath = @".\Assets\Data\Countries\" + countries[i].name + ".txt";
            using (StreamWriter file = new StreamWriter(filePath, mode))
            {
                if (day == 0)
                {
                    file.WriteLine(MAX_DAY);
                }
                else
                {
                    file.WriteLine(toWriteCountries[i]);
                }
            }
            filePath = @".\Assets\Data\Currencies\" + currencies[i].name + ".txt";
            using (StreamWriter file = new StreamWriter(filePath, mode))
            {
                if (day == 0)
                {
                    file.WriteLine(MAX_DAY);
                }
                else
                {
                    file.WriteLine(toWriteCurrencies[i]);
                }
            }
        }
        toWriteCountries = new List<string>();
        toWriteCurrencies = new List<string>();
        for (int i = 0; i < SERVICE_COUNT; i++)
        {
            string filePath = @".\Assets\Data\Services\" + services[i].name + ".txt";
            using (StreamWriter file = new StreamWriter(filePath, mode))
            {
                if (day == 0)
                {
                    file.WriteLine(MAX_DAY);
                }
                else
                {
                    file.WriteLine(toWriteServices[i]);
                }
            }
        }
        toWriteServices = new List<string>();
        for (int i = 0; i < PEOPLE_COUNT; i++)
        {
            string filePath = @".\Assets\Data\People\" + people[i].name + ".txt";
            using (StreamWriter file = new StreamWriter(filePath, mode))
            {
                if (day == 0)
                {
                    file.WriteLine(MAX_DAY);
                }
                else
                {
                    file.WriteLine(toWritePeople[i]);
                }
            }
        }
        toWritePeople = new List<string>();
    }
    void appendToCountryStrings(int day)
    {
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            if (day % BLOCK_SIZE == 0)
            {
                toWriteCountries.Add("");
            }
            else
            {
                toWriteCountries[i] += "\n";
            }
            toWriteCountries[i] += string.Format("{0} {1} {2} {3} {4} {5}", day, countries[i].Balance.ToString("N2"), countries[i].Gdp.ToString("N2"), (countries[i].Balance * countries[i].Currency.ExchangeRate[currencies[0]]).ToString("N2"), (countries[i].Gdp * countries[i].Currency.ExchangeRate[currencies[0]]).ToString("N2"), countries[i].Inflation);
        }
    }
    void appendToCurrencyStrings(int day)
    {
        for (int i = 0; i < COUNTRY_COUNT; i++)
        {
            if (day % BLOCK_SIZE == 0)
            {
                toWriteCurrencies.Add("");
            }
            else
            {
                toWriteCurrencies[i] += "\n";
            }
            toWriteCurrencies[i] += string.Format("{0} {1} {2} {3}", day, currencies[i].Value.ToString("N2"), currencies[i].Demand.ToString("N2"), currencies[i].Supply.ToString("N2"));
        }
    }
    void appendToServiceStrings(int day)
    {
        for (int i = 0; i < SERVICE_COUNT; i++)
        {
            if (day % BLOCK_SIZE == 0)
            {
                toWriteServices.Add("");
            }
            else
            {
                toWriteServices[i] += "\n";
            }
            toWriteServices[i] += string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", day, services[i].BasePrice.ToString("N2"), services[i].Price.ToString("N2"), (services[i].Price * services[i].OriginCountry.Currency.ExchangeRate[currencies[0]]).ToString("N2"), services[i].Demand.ToString("N2"), services[i].Supply.ToString("N2"), Service.Services_bought, (int)services[i].Type_of_supply);
        }
    }
    void appendToPeopleStrings(int day)
    {
        for (int i = 0; i < PEOPLE_COUNT; i++)
        {
            if (day % BLOCK_SIZE == 0)
            {
                toWritePeople.Add("");
            }
            else
            {
                toWritePeople[i] += "\n";
            }
            toWritePeople[i] += string.Format("{0} {1} {2} {3} {4} {5} {6}", day, people[i].Balance.ToString("N2"), (People[i].Balance * people[i].Country.Currency.ExchangeRate[currencies[0]]).ToString("N2"), people[i].SaveUrge.ToString("N2"), people[i].Country, people[i].Age.ToString("N2"), people[i].BoughtServices);
        }
    }

    void writeSettings()
    {
        string filePath = @".\Assets\Data\settings.txt";
        using (StreamWriter file = new StreamWriter(filePath, false))
        {
            file.WriteLine(MAX_DAY);
            file.WriteLine(COUNTRY_COUNT);
            file.WriteLine(COUNTRY_COUNT);
            file.WriteLine(PEOPLE_COUNT);
            file.WriteLine(SERVICE_COUNT);
        }
    }

    /// GETTER SETTERS
    public static List<Country> Countries { get => countries; set => countries = value; }
    public static List<Currency> Currencies { get => currencies; set => currencies = value; }
    public static List<Service> Services { get => services; set => services = value; }
    public static List<Person> People { get => people; set => people = value; }
}