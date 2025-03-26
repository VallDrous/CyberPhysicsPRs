from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression

import pandas as pd
import matplotlib.pyplot as plt

from sqlalchemy import create_engine
from sqlalchemy import text
class MachineLearning:
    def train(self, data, nameOfKey):
        defused = data[nameOfKey]
        features = data.drop(nameOfKey, axis=1)
        X_train, X_test, y_train, y_test = train_test_split(features,defused,test_size=0.2,random_state=10)
        self.regr = LinearRegression()
        self.regr.fit(X_train, y_train)

    def predictdF(self, df):
        res = self.regr.predict(df)
        print(res)
        return res

def getData(columnsToDelete, dropNameColumn):
    file_path = "pr3CPS\GlobalWeatherRepository.csv"

    df = pd.read_csv(file_path)

    dfEK = df.loc[df["timezone"] == "Europe/Kiev"]

    dfEK.reset_index(drop=True, inplace=True)

    dfEK = dfEK.drop(columns=columnsToDelete)

    dfEK[['Year', 'Month', 'DayTime']] = dfEK['last_updated'].str.split('-', expand=True, n=2)
    dfEK[['Day', 'Time']] = dfEK['DayTime'].str.split(' ', expand=True, n=1)

    dfEK['Year'] = pd.to_numeric(dfEK['Year'])
    dfEK['Month'] = pd.to_numeric(dfEK['Month'])
    dfEK['Day'] = pd.to_numeric(dfEK['Day'])

    dfEK = dfEK.drop(['DayTime', 'Time', 'Year', 'last_updated'], axis=1)


    print(dfEK)
    print(dfEK[['Day','Month']])
    dfEKWithoutKey = dfEK.drop(dropNameColumn, axis = 1)

    print(dfEKWithoutKey)

    return dfEK, dfEKWithoutKey

def toLearnAndGetDataForGrapth(dfEK, dfEKWithoutKey, nameColumnPredict, columnToBuildGraph):
    mL = MachineLearning()
    mL.train(dfEK, nameColumnPredict)
    genInf = mL.predictdF(dfEKWithoutKey)
    dfForGraph = pd.DataFrame(dfEK[columnToBuildGraph])
    dfGenInf = pd.DataFrame({nameColumnPredict : genInf})
    print(len(genInf))
    print(dfForGraph)
    return pd.concat([dfForGraph, dfGenInf], axis=1)


############################################################################################################

def operationWithWindKph():
    columns_to_delete_for_wind = ['country', 'location_name', 'latitude', 'longitude', 'timezone', 'last_updated_epoch',
                    'condition_text', 'wind_mph', 'wind_degree', 'wind_direction',
                      'pressure_in', 'precip_mm', 'precip_in', 'visibility_miles', 'uv_index', 'gust_mph',
                      'air_quality_Carbon_Monoxide', 'air_quality_Ozone', 'air_quality_Nitrogen_dioxide',
                      'air_quality_Sulphur_dioxide', 'air_quality_PM2.5', 'air_quality_PM10', 
                      'air_quality_us-epa-index', 'air_quality_gb-defra-index', 'moon_illumination', 
                      'moon_phase', 'moonset', 'moonrise', 'sunrise', 'sunset', 'feels_like_fahrenheit', 
                      'temperature_fahrenheit'] 
      
    dfEK, dfEKWithoutKey = getData(columns_to_delete_for_wind,"wind_kph")

    dfForGraph = toLearnAndGetDataForGrapth(dfEK, dfEKWithoutKey, "wind_kph", ['Day','Month'])
    print(dfForGraph)  
    dfToReturn = dfForGraph.copy()  
    grapthForWindKph(dfForGraph)

    return dfToReturn

def grapthForWindKph(dfForGraph):

    dfForGraph['Date'] = dfForGraph['Day'].astype(str) + '-' + dfForGraph['Month'].astype(str)

    def wind_color(wind_speed):
        if wind_speed < 10:
            return 'blue' 
        elif 10 <= wind_speed <= 20:
            return 'yellow' 
        else:
            return 'red'  

    dfForGraph['Color'] = dfForGraph['wind_kph'].apply(wind_color)

    dfForGraph = dfForGraph.sort_values(by=['Month', 'Day'])

    plt.figure(figsize=(12, 6))

    for i in range(len(dfForGraph) - 1):
        xValues = [dfForGraph['Date'].iloc[i], dfForGraph['Date'].iloc[i+1]]
        yValues = [dfForGraph['wind_kph'].iloc[i], dfForGraph['wind_kph'].iloc[i+1]]
        color = dfForGraph['Color'].iloc[i]
        plt.plot(xValues, yValues, color=color, linestyle='-', linewidth=2)  
        plt.scatter(xValues, yValues, color=color, edgecolors='black', zorder=3)  

    plt.xlabel("Дата")
    plt.ylabel("Швидкість вітру (км/год)")
    plt.title("Швидкість вітру за датами")
    plt.xticks(rotation=45)
    plt.grid(True)
    plt.show()



############################################################################################################




############################################################################################################

def operationWithTemperaturefeels():
    columns_to_delete_for_temperature = [
        'country', 'location_name', 'timezone', 'last_updated_epoch',
        'condition_text', 'wind_mph', 'wind_degree', 'wind_direction',
        'pressure_in', 'precip_in', 'visibility_miles', 'gust_mph',
        'air_quality_Carbon_Monoxide', 'air_quality_Ozone', 'air_quality_Nitrogen_dioxide',
        'air_quality_Sulphur_dioxide', 'air_quality_PM2.5', 'air_quality_PM10', 
        'air_quality_us-epa-index', 'air_quality_gb-defra-index', 'moon_illumination', 
        'moon_phase', 'moonset', 'moonrise', 'sunrise', 'sunset', 'feels_like_fahrenheit', 
        'temperature_fahrenheit',
        'latitude', 'longitude', 'pressure_mb', 'precip_mm', 'humidity', 'cloud', 
        'visibility_km', 'uv_index', 'gust_kph'
    ]

    dfEK, dfEKWithoutKey = getData(columns_to_delete_for_temperature,"feels_like_celsius")

    dfForGraph = toLearnAndGetDataForGrapth(dfEK, dfEKWithoutKey, "feels_like_celsius", ['Day','Month','temperature_celsius'])

    print(dfForGraph)
    dfToReturn = dfForGraph.copy()
    graphForTemperature(dfForGraph)

    return dfToReturn








def graphForTemperature(dfForGraph):
    dfForGraph['Date'] = dfForGraph['Day'].astype(str) + '-' + dfForGraph['Month'].astype(str)

    dfForGraph['feels_like_minus_real'] = dfForGraph['feels_like_celsius'] - dfForGraph['temperature_celsius']

    plt.figure(figsize=(12, 6))
    plt.plot(dfForGraph['Date'], dfForGraph['feels_like_minus_real'], label="Різниця (відчувається - реальна)", marker='o', linestyle='-', color='purple')

    plt.xlabel("Дата")
    plt.ylabel("Різниця температур (°C)")
    plt.title("Різниця між відчутною температурою та реальною")
    plt.xticks(rotation=45)
    plt.grid(True)

    plt.show()

############################################################################################################




def getMaxNumberForAddNewPredict(table):

    engine = create_engine(f"mysql+mysqlconnector://root:1234554321Sss@127.0.0.1/history_data_for_wind_kph")

    with engine.connect() as conn:
        result = conn.execute(text("SELECT MAX(number_of_predict) FROM "+ table))
        max_value = result.fetchone()[0]

    if max_value is None:
        max_value = 0
    engine.dispose()

    return max_value + 1

def addToDB(table, dfForDB):
    engine = create_engine(f"mysql+mysqlconnector://root:1234554321Sss@127.0.0.1/history_data_for_wind_kph")

    dfForSQL = dfForDB.rename(columns={"Day": "data_day", "Month": "data_month"})
    dfForSQL["number_of_predict"] = getMaxNumberForAddNewPredict(table)
    dfForSQL.to_sql(table, engine, if_exists="append", index=False)

    engine.dispose()

############################################################################################################

def showHistoryDataGraph(query):
    engine = create_engine(f"mysql+mysqlconnector://root:1234554321Sss@127.0.0.1/history_data_for_wind_kph")

    df = pd.read_sql(query, engine)
    print(df)

    engine.dispose()

    return df


############################################################################################################


def menuGraphBDGenerateDF(query):
    print("Напишіть число для вибору потрібної частини історичних даних")
    nHistory = input()
    df = showHistoryDataGraph(text(query + nHistory))
    return df




exit = True
while(exit):
    print("1.Зпрогнозувати швидкість відтру\n 2.Зпрогнозувати відчуття температури відносно вітру\n 3.Побудувати графік історичних даних по швидкості вітру\n 4.Побудувати графік історичних даних по відчуттю темпаратури відносно вітру \n 5.Вихід")
    number = input()
    if(number == "1"):
        dfForDB = operationWithWindKph()
        print("1.Додати до бд 2.Не додавати")
        numberForAddToDB=input()
        if(numberForAddToDB == "1"):
            addToDB("wind_kph", dfForDB)
        elif(numberForAddToDB == "2"):
            pass
        else:
           print("Ви ввели щось не так") 
    elif(number == "2"):
        dfForDB = operationWithTemperaturefeels()
        print("1.Додати до бд 2.Не додавати")
        numberForAddToDB=input()
        if(numberForAddToDB == "1"):
            addToDB("temperature_fells_like", dfForDB)
        elif(numberForAddToDB == "2"):
            pass
        else:
           print("Ви ввели щось не так") 
    elif(number == "3"):
        grapthForWindKph(menuGraphBDGenerateDF("select data_day as Day, data_month as Month, wind_kph from wind_kph where number_of_predict = "))
    elif(number == "4"):
        graphForTemperature(menuGraphBDGenerateDF("select data_day as Day, data_month as Month, temperature_celsius, feels_like_celsius from temperature_fells_like where number_of_predict = "))
    elif(number == "5"):
        exit = False
    else:
        print("Ви ввели щось не так")


############################################################################################################