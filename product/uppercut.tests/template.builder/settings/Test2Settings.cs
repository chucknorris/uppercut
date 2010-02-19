namespace uppercut.tests.template.builder.settings
{
    public class Test2Settings
    {
        public static string Contents =
            @"<project>
    <property name=""environment"" value=""TEST2"" />
    <property name=""server.database"" value=""dbtest2server"" />
    <property name=""server.app"" value=""apptest2server"" />
    <property name=""server.queue"" value=""apptest2server"" />
    <property name=""server.files"" value=""apptest2server"" />
    <property name=""server.mtpubsub"" value=""apptest2server"" />
    <property name=""mtpubsub.queue.name"" value=""mt_pubsub"" />
    
    <property name=""dirs.app.toplevel"" value=""\\${server.app}\Apps"" />
    <property name=""dirs.files.toplevel"" value=""\\${server.files}\Files"" />
    
    <property name=""log.level"" value=""DEBUG"" />
    <property name=""email.errors.to"" value=""someone@test2.com"" />
    
    <!-- A HOST -->
    <property name=""a.queue.name"" value=""aintegration"" />
    <property name=""a.bondservice.type"" value=""TEST2BondService"" />
    <property name=""a.int.time_between_queries"" value=""3600"" />
    <property name=""a.int.trades_days_after_settlement"" value=""45"" />
    
    <!-- B HOST -->
    <property name=""a.b.bondservice.type"" value=""TEST2BondService"" />
    <property name=""b.filewatch.interval"" value=""10"" />
    <property name=""b.queue.name"" value=""bintegration"" />
    <property name=""c.bondservice.type"" value=""TEST2BondService"" />
    <property name=""c.swapservice.type"" value=""TEST2SwapService"" />
    <property name=""c.discountnoteservice.type"" value=""TEST2DiscountNoteService"" />
    <property name=""b.filter.maturitydate"" value=""${'${NullFilter}'}"" />
    <property name=""b.filter.terminationdate"" value=""${'${NullFilter}'}"" />
    <property name=""c.terminationdatefilter.days"" value=""8"" />
    <property name=""b.merger.oof.filter"" value=""${'${NullFilter}'}"" />
    <property name=""b.oof.nonmerged.settledatefilter.days"" value=""5"" />
    <property name=""b.folders.archive.path"" value=""${dirs.files.toplevel}\BIntegration\Archive"" />

    <!-- Installation -->
    <property name=""b.app.name"" value=""BIntegration"" />
    <property name=""b.database.name"" value=""BDb"" />
    
    <!-- DOH! Castle Service Locaters look like properties!!! -->
    <property name=""MT.ListenEndpoint"" value=""${'${MT.ListenEndpoint}'}"" />
    <property name=""MT.LocalSubscriptionCache"" value=""${'${MT.LocalSubscriptionCache}'}"" />
    <property name=""MT.ServiceBus"" value=""${'${MT.ServiceBus}'}"" />
    <property name=""MT.SubscriptionEndPoint"" value=""${'${MT.SubscriptionEndPoint}'}"" />
    <property name=""BondTradeTypeRunner"" value=""${'${BondTradeTypeRunner}'}"" />
    <property name=""ARepository"" value=""${'${ARepository}'}"" />
    <property name=""ABondDatabasePublisher"" value=""${'${ABondDatabasePublisher}'}"" />
    <property name=""ACusipComparison"" value=""${'${ACusipComparison}'}"" />
    <property name=""ATradeDateComparison"" value=""${'${ATradeDateComparison}'}"" />
    <property name=""ASettleDateComparison"" value=""${'${ASettleDateComparison}'}"" />
    <property name=""ACounterpartyComparison"" value=""${'${ACounterpartyComparison}'}"" />
    <property name=""NullFilter"" value=""${'${NullFilter}'}"" />
    <property name=""SwapTypeRunner"" value=""${'${SwapTypeRunner}'}"" />
    <property name=""DiscountNoteTypeRunner"" value=""${'${DiscountNoteTypeRunner}'}"" />
    <property name=""TerminationDateFilter"" value=""${'${TerminationDateFilter}'}"" />
    <property name=""MaturityDateFilter"" value=""${'${MaturityDateFilter}'}"" />
    <property name=""SettleDateFilter"" value=""${'${SettleDateFilter}'}"" />
    <property name=""BondPublisher"" value=""${'${BondPublisher}'}"" />
    <property name=""SwapPublisher"" value=""${'${SwapPublisher}'}"" />
    <property name=""DiscountNotePublisher"" value=""${'${DiscountNotePublisher}'}"" />
    <property name=""BBondMerger"" value=""${'${BBondMerger}'}"" />
    <property name=""BBondMerger-Actual"" value=""${'${BBondMerger-Actual}'}"" />
    <property name=""BBondMergerValidator"" value=""${'${BBondMergerValidator}'}"" />
    <property name=""BCusipComparison"" value=""${'${BCusipComparison}'}"" />
    <property name=""BSettleDateComparison"" value=""${'${BSettleDateComparison}'}"" />
    <property name=""BCounterpartyComparison"" value=""${'${BCounterpartyComparison}'}"" />
    <property name=""BParAmountComparison"" value=""${'${BParAmountComparison}'}"" />
    <property name=""BReceiveCouponPercentComparison"" value=""${'${BReceiveCouponPercentComparison}'}"" />
    <property name=""BSeriesNumberComparison"" value=""${'${BSeriesNumberComparison}'}"" />
    <property name=""BBondFirstRoundComparisonList"" value=""${'${BBondFirstRoundComparisonList}'}"" />
    <property name=""BBondSecondRoundComparisonList"" value=""${'${BBondSecondRoundComparisonList}'}"" />
    <property name=""BgBondThirdRoundComparisonList"" value=""${'${BBondThirdRoundComparisonList}'}"" />
    <property name=""BBondFourthRoundComparisonList"" value=""${'${BBondFourthRoundComparisonList}'}"" />
</project>";
    }
}