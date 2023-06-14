import matplotlib.pyplot as plt
import numpy as np


for i in range(10):
    days = np.array([])
    data = np.array([])
    filename = "./Assets/Data/Countries/Country %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for j in range(int(a[0])):
        temp = [
            float(k.replace(".", "").replace(",", "."))
            for k in a[j + 1].strip().split(" ")
        ]
        days = np.append(days, temp[0])
        # 1-balance, 2-gdp, 3-balance in curr0, 4-gdp in curr0, 5-inflation
        data = np.append(data, temp[5])
    plt.plot(days, data, label="Country " + str(i))

plt.legend()
plt.show()
