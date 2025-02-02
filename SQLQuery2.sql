use pharmacy;

CREATE TABLE Capsules (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    Output VARCHAR(255) NOT NULL,
    CapsuleSizeMM DECIMAL(5,2) NOT NULL,
    MachineDimension VARCHAR(255) NOT NULL,
    ShippingWeightKG DECIMAL(10,2) NOT NULL,
    ImageURL VARCHAR(500) NOT NULL
);


select * from Capsules;
select * from Tablets;
select * from LiquidFilling;

INSERT INTO Capsules (ProductName, Output, CapsuleSizeMM, MachineDimension, ShippingWeightKG, ImageURL)
VALUES 
    ('Vitamin C Capsules', '5000 capsules/hour', 16.00, '1200x800x1500 mm', 250.50, 'https://www.nutrifactor.com.pk/cdn/shop/files/Extra-C-90_3260b3f1-4830-4c06-89e4-9f32ca1acc44_large.png?v=1737550517'),
    ('Protein Capsules', '7000 capsules/hour', 18.50, '1400x900x1600 mm', 300.75, 'https://static-01.daraz.pk/p/aaed12fb774041ee2b7c031e7bd725c3.png');



	CREATE TABLE Tablets (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ModelNumber VARCHAR(100) NOT NULL,
    Dies VARCHAR(50) NOT NULL,
    MaxPressure DECIMAL(10,2) NOT NULL,
    MaxDiameterMM DECIMAL(5,2) NOT NULL,
    MaxDepthFillMM DECIMAL(5,2) NOT NULL,
    ProductionCapacity VARCHAR(255) NOT NULL,
    MachineSize VARCHAR(255) NOT NULL,
    NetWeightKG DECIMAL(10,2) NOT NULL,
    ImageURL VARCHAR(500) NOT NULL
);

INSERT INTO Tablets (ModelNumber, Dies, MaxPressure, MaxDiameterMM, MaxDepthFillMM, ProductionCapacity, MachineSize, NetWeightKG, ImageURL)
VALUES 
    ('TP-1000', '1 set', 50.00, 12.50, 8.00, '5000 tablets/hour', '1200x800x1600 mm', 450.75, 'https://thepharmacyservices.com/wp-content/uploads/2023/11/T-P-S-2021-08-13T112301.053.jpg'),
    ('TP-2000', '2 set', 75.00, 16.00, 10.00, '8000 tablets/hour', '1400x900x1800 mm', 550.60, 'https://5.imimg.com/data5/SELLER/Default/2022/10/UL/ZX/GJ/4619647/etoricoxib-thiocolchicoside-tablet.jpeg');



	CREATE TABLE LiquidFilling (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ModelName VARCHAR(255) NOT NULL,
    AirPressure DECIMAL(10,2) NOT NULL,
    AirVolume VARCHAR(100) NOT NULL,
    FillingSpeed INT NOT NULL, 
    FillingRangeML VARCHAR(100) NOT NULL,
    ImageURL VARCHAR(500) NOT NULL
);

INSERT INTO LiquidFilling (ModelName, AirPressure, AirVolume, FillingSpeed, FillingRangeML, ImageURL)
VALUES 
    ('DHF-100', 6.5, '10 L/min', 120, '50-500 ml', 'https://image.made-in-china.com/202f0j00OZeWctSlhQfr/Automatic-Plastic-Bottle-Liquid-Power-Wash-Dish-Spray-Dish-Soap-Water-Packaging-Filling-Machine.jpg'),
    ('DHF-200', 7.0, '12 L/min', 150, '100-1000 ml', 'https://image.made-in-china.com/2f0j00vLFoGYRMLnkw/Gzd-200-High-Accuracy-Desktop-Double-Head-Water-Juice-Beverage-Wine-Oil-Bottle-Filler-Gear-Pump-Liquid-Filling-Machine.webp');
