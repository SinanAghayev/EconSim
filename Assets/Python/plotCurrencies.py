import matplotlib.pyplot as plt
import numpy as np

for i in range(10):
    days = np.array([])
    data = np.array([])
    filename = "./Assets/Data/Currencies/Currency %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for j in range(int(a[0])):
        temp = [
            float(k.replace(".", "").replace(",", "."))
            for k in a[j + 1].strip().split(" ")
        ]
        days = np.append(days, temp[0])
        # 1-value, 2-demand, 3-supply
        data = np.append(data, temp[3])
    plt.plot(days, data, label="Currency " + str(i))

plt.legend()
plt.show()
