# CurrencyExchangeSystem

# Instructions:
-    git clone https://github.com/Roeyas/CurrencyExchangeSystem.
-    open the .Sln file.
-    right-click on the solution in the Solution Explorer and select Restore NuGet Packages. (or type Restore-Project on console).
-    Update Connection string for data access here: SystemSimulator/App.config (either for windows authentication or Server authentication).
-    Create the Database 'CurrencyExchange' and run SetupDatabase.sql.

# API's
- XE Currency - return the value 1.235 for all currencies (unknown reason).
- OpenExchange - allow convertions only from USD to X (limited API).

# Flow
- For smooth flow (No backup data source use) - set "CurrencyPairs" in App.config as  <add key="CurrencyPairs" value="USD/ILS,USD/EUR,USD/GBP,USD/JPY" />
- For fallback (using second data source) - set "CurrencyPairs" as requested in App.config as <add key="CurrencyPairs" value="USD/ILS,USD/EUR,GBP/EUR,EUR/JPY" />
- To start Run SystemSimulator console.


בחרתי ב-Database כי הרבה יותר קל לנהל את הדאטה מכמה בחינות.
1) סכמה מסודרת ומנורמלת.
2) הדאטה בייס מכיל יתרונות מבחיינת אופטימיזציות של יעילות וביצועים.
3) רלציוני - קל להיתנהל איתו בעזרת שאילתות ופרוצדורות.
4) מתאים יותר כאשר המערכת גודלת.
5) שלמות המידע - עובד לפי טרנזקציות, כאשר טרנזקציה נכשחת באמצע יש לו אפשרות לחזור למצב הקודם לפני הטרנזקציה.
