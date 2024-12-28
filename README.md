# CurrencyExchangeSystem

# Instructions:
-    git clone https://github.com/Roeyas/CurrencyExchangeSystem
-    Update Connection string for data access here: SystemSimulator/App.config (either for windows authentication or Server authentication)
-    Create the Database 'CurrencyExchange' and run SetupDatabase.sql.

# API's
- XE Currency - return the value 1.235 for all currencies (unknown reason)
- OpenExchange - allow convertions only from USD to X (limited API)

# Flow
- For smooth flow (No backup data source use) - set "CurrencyPairs" in App.config as  <add key="CurrencyPairs" value="USD/ILS,USD/EUR,USD/GBP,USD/JPY" />
- For fallback (using second data source) - set "CurrencyPairs" as requested in App.config as <add key="CurrencyPairs" value="USD/ILS,USD/EUR,GBP/EUR,EUR/JPY" />
