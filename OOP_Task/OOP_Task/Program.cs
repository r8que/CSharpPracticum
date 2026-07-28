var nameList = new List<string>()
{
    "Протеиновый батончик",
    "Кофе растворимый",
    "Гель для душа",
    "Чипсы",
    "Мёд цветочный"
};

var manufacturerList = new List<string>()
{
    "FitPower",
    "BrewMaster",
    "AquaCare",
    "SnackPro",
    "BeeHarmony"
};

var priceList = new List<double>()
{
    185,
    420,
    290,
    130,
    650
};

var expDateList = new List<string>()
{
    "365",
    "730",
    "540",
    "180",
    "320"
};

var prodDateList = new List<string>()
{
    "10.07.2026",
    "01.03.2026",
    "15.05.2026",
    "20.06.2026",
    "05.08.2025"
};

var random = new Random();
Product randProd = new Product(nameList[random.Next(nameList.Count)], manufacturerList[random.Next(manufacturerList.Count)], priceList[random.Next(priceList.Count)], expDateList[random.Next(expDateList.Count)], prodDateList[random.Next(prodDateList.Count)]);
Console.Write(randProd.ToString());