# XESmartTarget

*XESmartTarget is the simplest way to write Extended Events data to a database table. But wait: there's more! XESmartTarget is the easiest way to process events without writing a single line of code!*

XESmartTarget is a command line tool to help you working with SQL Server Extended Events. It offers the ability to perform custom actions in response to Events, in a way that is not possible by using the built-in targets.

While you are free to extend XESmartTarget with your own Response types, XESmartTarget does not require any coding. Instead, it can be configured with simple `.json` configuration files.

For instance, the following configuration file instructs XESmartTarget to connect to the server `(local)`, hook to the Extended Events session `test_session` and forward all the events of type `sql_batch_completed` to a Response of type `TableAppenderResponse`, which will insert all events every `10 seconds` into a table named `test_session_data` in the server `(local)`, Database `XESmartTargetTest`, only the columns specified, only the rows with duration > 10000 microseconds.
<BR>It will also replay the `sql_batch_completed` events to the instance `(local)\SQL2014` using the `ReplayResponse` Response type.

```javascript
{
    "Target": {
        "ServerName": "(local)",
        "SessionName": "test_session",
        "Responses": [
            {
                //  JSON config files can contain comments
                "__type": "TableAppenderResponse",
                "ServerName": "(local)",
                "DatabaseName": "XESmartTargetTest",
                "TableName": "test_session_data",
                "AutoCreateTargetTable": true,
                "UploadIntervalSeconds": 10,
                "Events": [
                    "sql_batch_completed"
                ],
                "OutputColumns": [
                    "cpu_time", 
                    "duration", 
                    "physical_reads", 
                    "logical_reads", 
                    "writes", 
                    "row_count", 
                    "batch_text"
                ],
                "Filter": "duration > 10000"
            },
            {
                "__type": "ReplayResponse",
                "ServerName": "(local)\\SQL2014",
                "DatabaseName": "XESmartTargetTest",
                "Events": [
                    "sql_batch_completed"
                ],
                "StopOnError" : false
            }
        ]
    }
}
```

A complete description of all the setting you can control in the `.JSON` input file can be found in the [Documentation](https://github.com/spaghettidba/XESmartTarget/wiki).

Here is the output it produces: 
![Screenshot 1](https://github.com/spaghettidba/XESmartTarget/blob/master/Images/Screenshot1.png?raw=true "Screenshot")

And here is the output it produces in the database:
![Screenshot 2](https://github.com/spaghettidba/XESmartTarget/blob/master/Images/Screenshot2.png?raw=true "Screenshot")

For the moment, the following Response types are available: 
* `CsvAppenderResponse`
* `EmailResponse`
* `ExecuteTSQLResponse`
* `ReplayResponse`
* `TableAppenderResponse`

New Response types are in the works, such as `GroupedTableAppenderResponse` (groups data before writing to a table). 
Suggestions for new Response Types are more than welcome.

Check out the [Documentation](https://github.com/spaghettidba/XESmartTarget/wiki) for more information.


# XelToCsv
XESmartTarget also includes a command line tool to convert .XEL files to .CSV. 

Usage is very simple. Here's an example:

```
XelToCsv.exe -s c:\temp\events.xel -d c:\temp\events.csv
```

# It's a Library!
XESmartTarget is a .dll that you can use in your project. Not sure how to use it? No problem, look at `Program.cs` in the XESmartTarget project. It's super easy.
