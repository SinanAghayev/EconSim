import matplotlib.pyplot as plt
import numpy as np

filenames = [
    "./Assets/Data/Countries/Country %d.txt",
    "./Assets/Data/Currencies/Currency %d.txt",
    "./Assets/Data/People/Person %d.txt",
    "./Assets/Data/Services/Service %d.txt",
]

name = [
    "Country ",
    "Currency ",
    "Person ",
    "Service ",
]

# country, currency, people, service
countOfItems = [20, 20, 500, 2000]

maxDays = 0

# Read settings
# Before definiton of functions because of maxDays.
with open("./Assets/Data/settings.txt", "r") as file:
    temp = file.readlines()
    maxDays = int(temp[0])
    for i in range(1, 5):
        countOfItems[i - 1] = int(temp[i])


def is_numeric(value):
    try:
        float(value)
        return True
    except ValueError:
        return False


def show(f, graphType, d=maxDays):
    # f is type of file to open
    # graphType is type of graph to show
    # d is day count to show
    print(d)
    for i in range(countOfItems[f]):
        days = np.array([])
        data = np.array([])
        filename = filenames[f] % (i)
        with open(filename, "r") as file:
            a = file.readlines()
        for j in range(d):
            temp = [
                float(k.replace(".", "").replace(",", "."))
                for k in a[j + 1].strip().split(" ")
                if is_numeric(k.replace(".", "").replace(",", "."))
            ]
            days = np.append(days, temp[0])
            data = np.append(data, temp[graphType])

        plt.plot(days, data, label=name[f] + str(i))

    plt.legend()
    plt.show()


while 1:
    inputs = [int(i) for i in input("\nChoose: ").strip().split()]
    if len(inputs) == 3:
        show(inputs[0], inputs[1], inputs[2])
    else:
        print(maxDays)
        show(inputs[0], inputs[1])

# MANUAL:
# 0 is country and 1-balance, 2-gdp, 3-balance in curr0, 4-gdp in curr0, 5-inflation
# 1 is currency and 1-value, 2-demand, 3-supply
# 2 is people and 1-balance, 2-balance in curr0, 3-saveUrge, 4-country, 5-age, 6-boughtServices
# 3 is service and 1-base price, 2-price, 3-price in curr0, 4-demand, 5-supply, 6-overall services bought, 7-type
