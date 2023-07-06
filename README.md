# EconSim

This repository contains the code for a simulation program written in C# using Unity. The program simulates economic interactions between countries and people. It also simulates the change in the values (price for services), demand, and supply of currencies and services.

# Requirements

Unity game engine

Python

# Installation

Clone this repository to your local machine or download the ZIP file and extract it.
Open the project in Unity.
Build and run the project in Unity to start the simulation.

# Usage

The main functionality of the simulation is implemented in the Main script, which serves as the entry point for the simulation. The Main script is attached to a game object in the Unity scene.

The simulation will automatically start when you run the simulation on Unity and stop after reaching the maximum number of days specified by the MAX_DAY constant.

During the simulation, various data will be written to files in the Assets/Data directory. Each country, currency, service, and person will have its own data file.

Use the Python script in the Python file to visualize the data written in the simulation.

# Visualization

After running the Python script, the program expects an array of numbers as input from you.

\* **Currency of the first (0 indexed) country is chosen as a comparison medium.**

# Customization

You can customize several parameters in the Main script:

COUNTRY_COUNT: Number of countries in the simulation.

SERVICE_COUNT: Number of services in the simulation.

PEOPLE_COUNT: Number of people in the simulation.

CEIL_PRICE: Maximum price for services.

MAX_PROSPERITY: Maximum prosperity level for countries.

COUNTRY_SIZE: Size of the country planes in the Unity scene.

MAX_DAY: Maximum number of simulation days.

BLOCK_SIZE: Interval for writing data to files.

You can modify these parameters to change the scale and duration of the simulation according to your requirements.

# Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please feel free to create an issue or submit a pull request.
