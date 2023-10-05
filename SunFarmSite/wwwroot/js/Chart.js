const MONTHS_YEAR = 12;

const LoadChart = (appData) => {
    if (Object.keys(appData).length === 0) { return; } // Make sure object is NOT empty

    const CHART_ID = 'sales-chart';
    const chartEl = document.getElementById('sales-chart');
    if (chartEl) {
        // Use themes
        // am4core.useTheme(am4themes_animated);

        // Create chart instance
        const chart = am4core.create(CHART_ID, am4charts.XYChart);
        chart.paddingRight = 20;

        // Create axes
        const dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        const valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

        dateAxis.dataFields.category = 'date';
        dateAxis.dateFormat = 'MMM';
        dateAxis.renderer.minGridDistance = 50;
        dateAxis.renderer.grid.template.location = 0.5;
        dateAxis.startLocation = 0.5;
        dateAxis.endLocation = 0.5;

        valueAxis.dataFields.valueY = 'value';
        valueAxis.title.text = 'Dollars';

        const salesSeries = chart.series.push(new am4charts.LineSeries());
        salesSeries.dataFields.dateX = 'date';
        salesSeries.dataFields.valueY = 'value';
        salesSeries.tensionX = 0.8;
        const salesBullet = salesSeries.bullets.push(new am4charts.CircleBullet());
        salesBullet.tooltipText = 'Sales {valueY}';
        salesSeries.stroke = am4core.color('rgb(0,255,0)');

        const returnsSeries = chart.series.push(new am4charts.LineSeries());
        returnsSeries.dataFields.dateX = 'date';
        returnsSeries.dataFields.valueY = 'value';
        returnsSeries.tensionX = 0.8;
        const returnsBullet = returnsSeries.bullets.push(new am4charts.CircleBullet());
        returnsBullet.tooltipText = 'Returns {valueY}';
        returnsSeries.stroke = am4core.color('rgb(255,0,0)');

        salesSeries.data = [];
        returnsSeries.data = [];

        for (let month = 0; month < MONTHS_YEAR; month++) {
            salesSeries.data.push({
                "date": new Date(appData.year, month), "value": appData.sales[month]
            });
            returnsSeries.data.push({
                "date": new Date(appData.year, month), "value": appData.returns[month]
            });
        }
    }
}