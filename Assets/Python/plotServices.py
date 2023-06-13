import matplotlib.pyplot as plt
import numpy as np


for i in range(50):
    months = np.array([])
    data = np.array([])
    filename = "./Assets/Data/Services/Service %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for j in range(int(a[0])):
        temp = [
            float(k.replace(".", "").replace(",", "."))
            for k in a[j + 1].strip().split(" ")
        ]
        months = np.append(months, temp[0])
        # 1-base price, 2-price, 3-price in curr0, 4-demand, 5-supply
        data = np.append(data, temp[3])
        if len(months) == 500:
            break
    plt.plot(months, data, label="Service " + str(i))

plt.legend()
plt.show()
