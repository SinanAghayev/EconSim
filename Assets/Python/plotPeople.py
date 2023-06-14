import matplotlib.pyplot as plt
import numpy as np

x = 0
max_ = 0
local_max = 0
for i in range(100):
    days = np.array([])
    data = np.array([])
    filename = "./Assets/Data/People/Person %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for j in range(int(a[0])):
        temp = a[j + 1].strip().split(" ")
        days = np.append(days, float(temp[0].replace(".", "").replace(",", ".")))
        # 1-balance, 2-balance in curr0, 3-saveUrge, 5-country, 7-age, 8-boughtServices
        data = np.append(data, float(temp[8].replace(".", "").replace(",", ".")))
        if float(temp[1].replace(".", "").replace(",", ".")) > local_max:
            local_max = float(temp[1].replace(".", "").replace(",", "."))
    print(i)
    if local_max > max_:
        max_ = local_max
        x = i
    plt.plot(days, data, label="Person " + str(i))

print(max_)
print(x)
plt.legend()
plt.show()
