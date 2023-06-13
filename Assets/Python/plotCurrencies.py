import matplotlib.pyplot as plt
import numpy as np

for i in range(10):
    months = np.array([])
    data = np.array([])
    filename = "./Assets/Data/Currencies/Currency %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for j in range(int(a[0])):
        temp = [
            float(k.replace(".", "").replace(",", "."))
            for k in a[j + 1].strip().split(" ")
        ]
        months = np.append(months, temp[0])
        # 1-value, 2-demand, 3-supply
        data = np.append(data, temp[3])
    plt.plot(months, data, label="Currency " + str(i))

plt.legend()
plt.show()
