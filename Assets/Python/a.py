import matplotlib.pyplot as plt
import numpy as np


for i in range(10):
    months = np.array([])
    data = np.array([])
    filename = "./Assets/Data/Countries/Country %d.txt" % (i)
    with open(filename, "r") as file:
        a = file.readlines()
    for elem in a:
        print(elem)
        temp = [
            float(k.replace(".", "").replace(",", ".")) for k in elem.strip().split(" ")
        ]
        months = np.append(months, temp[0])
        data = np.append(data, temp[1])
