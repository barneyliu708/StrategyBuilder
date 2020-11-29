using Microsoft.EntityFrameworkCore.Migrations;

namespace StrategyBuilder.Repository.Migrations
{
    public partial class AddEarningFillingDateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Insert Into [Users] (Username, EncryptedPassword, SecretKey)
                                   Values('userdemo1', 'test1', 'test'),
                                         ('userdemo2', 'test2', 'test')
                                   
                                   Insert Into [Strategies] ([Name], [Description], CreatedById)
                                   Values ('Earnings Events Driven Trading Strategy', 'This trading trategy is trying to find out the impact of the events when companies release their quaterly earning report and find the trading opportunities.', IDENT_CURRENT('Users'))
                                   
                                   Insert Into [EventGroups] ([Name], [Description], [CreatedById], [StrategyId])
                                   Values ('AAPL Earnings Filling Dates', 'A quarterly earnings report is a quarterly filing made by public companies to report their performance. Earnings reports include items such as net income, earnings per share, earnings from continuing operations, and net sales. By analyzing quarterly earnings reports, investors can begin to gauge the financial health of the company and determine whether it deserves their investment.', IDENT_CURRENT('Users'), IDENT_CURRENT('Strategies'))
                                   
                                   Insert Into [Events] ([Occurrence], [EventGroupId])
                                   Values 
                                   ('2020-10-29 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-07-30 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-04-30 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-01-28 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-10-30 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-07-30 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-04-30 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-01-29 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-11-01 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-07-31 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-05-01 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-02-01 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-11-02 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-08-01 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-05-02 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-01-31 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-10-25 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-07-26 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-04-26 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-01-26 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-10-27 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-07-21 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-04-27 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-01-27 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2014-10-20 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2014-07-22 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2014-04-23 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2014-01-27 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2013-10-28 00:00:00.0000000', IDENT_CURRENT('EventGroups'))
                                   
                                   
                                   Insert Into [Strategies] ([Name], [Description], CreatedById)
                                   Values ('Dividend Announcement Event Trading Strategy', 'The declaration date is the date on which the board of directors of a company announces the next dividend payment. This statement includes the dividend size, ex-dividend date, and payment date. The declaration date is also referred to as the ""announcement date.""', IDENT_CURRENT('Users'))
                                   
                                   Insert Into[EventGroups]([Name], [Description], [CreatedById], [StrategyId])
                                   Values('AAPL Dividend History', 'AAPL dividend historical declaration date', IDENT_CURRENT('Users'), IDENT_CURRENT('Strategies'))
                                   
                                   Insert Into[Events] ([Occurrence], [EventGroupId])
                                   Values
                                   ('2020-11-06 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-08-07 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-05-08 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-02-07 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-11-07 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-08-09 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-05-10 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-02-08 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-11-08 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-08-10 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-05-11 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-02-09 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-11-10 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-08-10 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-05-11 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-02-09 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-11-03 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-08-04 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-05-05 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-02-04 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-11-05 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-08-06 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-05-07 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-02-05 00:00:00.0000000', IDENT_CURRENT('EventGroups'))
                                   
                                   Insert Into[Strategies] ([Name], [Description], CreatedById)
                                   Values('Fed Fund Rates Change Event Trading Strategy', 'Although the relationship between interest rates and the stock market is fairly indirect, the two tend to move in opposite directions—as a general rule of thumb, when the Fed cuts interest rates, it causes the stock market to go up and when the Fed raises interest rates, it causes the stock market as a whole to go down. But there is no guarantee of how the market will react to any given interest rate change the Fed chooses to make.', IDENT_CURRENT('Users'))
                                   
                                   Insert Into[EventGroups] ([Name], [Description], [CreatedById], [StrategyId])
                                   Values('Federal Funds Fate Changes (Decrease)', 'In the United States, the federal funds rate is the interest rate at which depository institutions lend reserve balances to other depository institutions overnight on an uncollateralized basis. Reserve balances are amounts held at the Federal Reserve to maintain depository institutions reserve requirements', IDENT_CURRENT('Users'), IDENT_CURRENT('Strategies'))
                                   
                                   Insert Into[Events] ([Occurrence], [EventGroupId])
                                   Values
                                   ('2020-03-16 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2020-03-03 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-10-31 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-09-19 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2019-08-01 00:00:00.0000000', IDENT_CURRENT('EventGroups'))
                                   
                                   Insert Into[EventGroups] ([Name], [Description], [CreatedById], [StrategyId])
                                   Values('Federal Funds Fate Changes (Increase)', 'In the United States, the federal funds rate is the interest rate at which depository institutions lend reserve balances to other depository institutions overnight on an uncollateralized basis. Reserve balances are amounts held at the Federal Reserve to maintain depository institutions reserve requirements', IDENT_CURRENT('Users'), IDENT_CURRENT('Strategies'))
                                   
                                   Insert Into[Events] ([Occurrence], [EventGroupId])
                                   Values
                                   ('2018-12-20 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-09-27 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-06-14 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2018-03-22 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-12-14 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-06-15 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2017-03-16 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2016-12-15 00:00:00.0000000', IDENT_CURRENT('EventGroups')),
                                   ('2015-12-17 00:00:00.0000000', IDENT_CURRENT('EventGroups'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Delete from [Events]
                                   Delete from [EventGroups]
                                   Delete from [Strategies]
                                   Delete from [Users]");
        }
    }
}
