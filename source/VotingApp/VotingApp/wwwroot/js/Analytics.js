$(document).ready(function () {

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {

        var chartData = document.getElementById('chartData').value;
        alert(chartData);

        for (var i = 0; i < chartData.length; i++) {
            alert([chartData[i].Key, chartData[i].Value]);
        }

        //var voteOptions = document.getElementById('chartVoteOptions').value;
        //alert(voteOptions);

        //for (var i = 0; i < voteOptions.length; i++) {
        //    alert(voteOptions[i]);
        //}

        //var votes = document.getElementById('chartVoteTotals').getAttribute('value');
        //alert(votes);

        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Vote Option');
        data.addColumn('number', 'Votes');
        //data.addRows([
        //    //['Pepperoni', 33],
        //    //['Hawaiian', 26],
        //    //['Mushroom', 22],
        //    //['Sausage', 10], 
        //    //['Anchovies', 9] 
        //]);

        //for (var i = 0; i < voteOptions.length; i++) {
        //    data.addRow([voteOptions[i], votes[i]]);
        //}

        for (var i = 0; i < chartData.length; i++) {
            data.addRow([chartData[i].Key, chartData[i].Value]);
        }

        var options = {
            title: 'Your Vote Results',
            pieSliceText: 'value',
            is3D: true
        };


        var chart = new google.visualization.PieChart(document.getElementById('piechart3d'));

        chart.draw(data, options);
    }


});