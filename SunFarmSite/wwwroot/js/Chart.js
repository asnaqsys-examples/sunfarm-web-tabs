const LoadChart = (appData) => {
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

        salesSeries.data = [
            { "date": new Date(appData.year, 0), "value": appData.sales[0] },
            { "date": new Date(appData.year, 1), "value": appData.sales[1] },
            { "date": new Date(appData.year, 2), "value": appData.sales[2] },
            { "date": new Date(appData.year, 3), "value": appData.sales[3] },
            { "date": new Date(appData.year, 4), "value": appData.sales[4] },
            { "date": new Date(appData.year, 5), "value": appData.sales[5] },
            { "date": new Date(appData.year, 6), "value": appData.sales[6] },
            { "date": new Date(appData.year, 7), "value": appData.sales[7] },
            { "date": new Date(appData.year, 8), "value": appData.sales[8] },
            { "date": new Date(appData.year, 9), "value": appData.sales[9] },
            { "date": new Date(appData.year, 10), "value": appData.sales[10] },
            { "date": new Date(appData.year, 11), "value": appData.sales[11] }
        ];
        returnsSeries.data = [
            { "date": new Date(appData.year, 0), "value": appData.returns[0] },
            { "date": new Date(appData.year, 1), "value": appData.returns[1] },
            { "date": new Date(appData.year, 2), "value": appData.returns[2] },
            { "date": new Date(appData.year, 3), "value": appData.returns[3] },
            { "date": new Date(appData.year, 4), "value": appData.returns[4] },
            { "date": new Date(appData.year, 5), "value": appData.returns[5] },
            { "date": new Date(appData.year, 6), "value": appData.returns[6] },
            { "date": new Date(appData.year, 7), "value": appData.returns[7] },
            { "date": new Date(appData.year, 8), "value": appData.returns[8] },
            { "date": new Date(appData.year, 9), "value": appData.returns[9] },
            { "date": new Date(appData.year, 10), "value": appData.returns[10] },
            { "date": new Date(appData.year, 11), "value": appData.returns[11] }
        ];
    }
}