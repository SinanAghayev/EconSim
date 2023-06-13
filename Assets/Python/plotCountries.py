import matplotlib.pyplot as plt
import numpy as np


for i in range(10):
    months = np.array([])
    data = np.array([])
    filename = "./Assets/Data/Countries/Country %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for j in range(int(a[0])):
        temp = [
            float(k.replace(".", "").replace(",", "."))
            for k in a[j + 1].strip().split(" ")
        ]
        months = np.append(months, temp[0])
        # 1-balance, 2-gdp, 3-balance in curr0, 4-gdp in curr0
        data = np.append(data, temp[2])
    plt.plot(months, data, label="Country " + str(i))

plt.legend()
plt.show()
