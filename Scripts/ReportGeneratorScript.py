import sys
import json

# load input arguments from the text file
filename = sys.argv[ 1 ]
#filename = "C:/Users/barne/OneDrive/Documents/CPT/HU/Semester 5/GRAD 695/Project/e6b87950-e68c-49e7-96ea-aef0898ebf4c.txt"

with open( filename ) as data_file:   
    input_args = json.loads( data_file.read() )

# read value from input args
filename, strategyname, strategydescription, symbol, eventNames, executedon, executefrom, executeto, x, y = [ input_args.get( key ) for key in [ 'filename', 'strategyname', 'strategydescription', 'symbol', 'eventNames', 'executedon', 'executefrom', 'executeto', 'x', 'y' ] ]

chartdata = []
for i in range(len(x)):
    chartdata.append((x[i], y[i]))


from reportlab.lib.styles import getSampleStyleSheet
from reportlab.lib.validators import Auto
from reportlab.graphics.charts.legends import Legend
from reportlab.graphics.charts.piecharts import Pie
from reportlab.graphics.charts.lineplots import LinePlot
from reportlab.graphics.shapes import Drawing, String
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer
from reportlab.graphics.widgets.markers import makeMarker
from reportlab.lib.colors import purple, PCMYKColor, black, pink, green, blue
from reportlab.rl_config import defaultPageSize
from reportlab.lib.units import inch

PAGE_HEIGHT=defaultPageSize[1]; PAGE_WIDTH=defaultPageSize[0]

def add_legend(draw_obj, chart, data):
    legend = Legend()
    legend.alignment = 'right'
    legend.x = 10
    legend.y = 70
    legend.colorNamePairs = Auto(obj=chart)
    draw_obj.add(legend)
def pie_chart_with_legend():
    data = list(range(15, 105, 15))
    drawing = Drawing(width=400, height=200)
    my_title = String(170, 40, 'My Pie Chart', fontSize=14)
    pie = Pie()
    pie.sideLabels = True
    pie.x = 150
    pie.y = 65
    pie.data = data
    pie.labels = [letter for letter in 'abcdefg']
    pie.slices.strokeWidth = 0.5
    drawing.add(my_title)
    drawing.add(pie)
    add_legend(drawing, pie, data)
    return drawing
def line_chart_with_legend():
    fontName = 'Helvetica'
    fontSize = 7
    data = [chartdata]
    drawing = Drawing(width=400, height=200)
    my_title = String(50, 120, 'Event Impact on Stock Price Change', fontSize=11)
    chart = LinePlot()
    # chart
    chart.y                = 16
    chart.x                = 32
    chart.width            = 212
    chart.height           = 90
    # line styles
    chart.lines.strokeWidth     = 0
    chart.lines.symbol= makeMarker('FilledSquare')
    
    # x axis
#     chart.xValueAxis = NormalDateXValueAxis()
    chart.xValueAxis.labels.fontName          = fontName
    chart.xValueAxis.labels.fontSize          = fontSize-1
#     chart.xValueAxis.forceEndDate             = 1
#     chart.xValueAxis.forceFirstDate           = 1
    chart.xValueAxis.labels.boxAnchor      ='autox'
#     chart.xValueAxis.xLabelFormat          = '{d}-{MMM}'
    chart.xValueAxis.maximumTicks          = 5
    chart.xValueAxis.minimumTickSpacing    = 0.5
#     chart.xValueAxis.niceMonth             = 0
    chart.xValueAxis.strokeWidth           = 1
    chart.xValueAxis.loLLen                = 5
    chart.xValueAxis.hiLLen                = 5
    chart.xValueAxis.gridEnd               = 258
    chart.xValueAxis.gridStart             = chart.x-10
    chart.xValueAxis.labelTextFormat       = '%d day'
    #chart.xValueAxis.setPosition(50, 50, 125)
    
    # y axis
          #self.chart.yValueAxis = AdjYValueAxis()
    chart.yValueAxis.visibleGrid           = 1
    chart.yValueAxis.visibleAxis=0
    chart.yValueAxis.labels.fontName       = fontName
    chart.yValueAxis.labels.fontSize       = fontSize -1
    chart.yValueAxis.labelTextFormat       = '%0.2f%%'
    chart.yValueAxis.strokeWidth           = 0.25
    chart.yValueAxis.visible               = 1
    chart.yValueAxis.labels.rightPadding   = 5
        #self.chart.yValueAxis.maximumTicks          = 6
    chart.yValueAxis.rangeRound            ='both'
    chart.yValueAxis.tickLeft              = 7.5
    chart.yValueAxis.minimumTickSpacing    = 0.5
    chart.yValueAxis.maximumTicks          = 8
    chart.yValueAxis.forceZero             = 0
    chart.yValueAxis.avoidBoundFrac = 0.1
    # sample data
    chart.data = data
    chart.lines[0].strokeColor = PCMYKColor(0,100,100,40,alpha=100)
    chart.lines[1].strokeColor = PCMYKColor(100,0,90,50,alpha=100)
    chart.xValueAxis.strokeColor             = PCMYKColor(100,60,0,50,alpha=100)
#     self.legend.colorNamePairs = [(PCMYKColor(0,100,100,40,alpha=100), 'Bovis Homes'), (PCMYKColor(100,0,90,50,alpha=100), 'HSBC Holdings')]
    chart.lines.symbol.x           = 0
    chart.lines.symbol.strokeWidth = 0
    chart.lines.symbol.arrowBarbDx = 5
    chart.lines.symbol.strokeColor = PCMYKColor(0,0,0,0,alpha=100)
    chart.lines.symbol.fillColor   = None
    chart.lines.symbol.arrowHeight = 5
    chart.lines.symbol.kind        = 'FilledCross'
    chart.lines.symbol.size        = 5
    chart.lines.symbol.angle       = 45
    
    drawing.add(my_title)
    drawing.add(chart)
    add_legend(drawing, chart, data)
    return drawing
Title = "Hello world"
pageinfo = "platypus example"
def myFirstPage(canvas, doc):
    canvas.saveState()
    canvas.setFont('Times-Bold',16)
    canvas.drawCentredString(PAGE_WIDTH/2.0, PAGE_HEIGHT-108, strategyname)
    canvas.setFont('Times-Roman',9)
    canvas.drawString(inch, 0.75 * inch, "First Page / %s" % pageinfo)
    canvas.restoreState()

def main():
    doc = SimpleDocTemplate(filename)
    
    elements = [Spacer(1,1*inch)]
    styles = getSampleStyleSheet()
    
    ptext = Paragraph('Description: ' + strategydescription, styles["Normal"])
    elements.append(ptext)
    elements.append(Spacer(1,0.2*inch))
    
    ptext = Paragraph('Executed On: ' + executedon, styles["Normal"])
    elements.append(ptext)
    elements.append(Spacer(1,0.2*inch))
    
    ptext = Paragraph('Backtesting Stock: ' + symbol, styles["Normal"])
    elements.append(ptext)
    ptext = Paragraph('Backtesting From: ' + executefrom, styles["Normal"])
    elements.append(ptext)
    ptext = Paragraph('Backtesting To: ' + executeto, styles["Normal"])
    elements.append(ptext)
    elements.append(Spacer(1,0.2*inch))
    
    ptext = Paragraph('Backtesting Events:', styles["Normal"])
    elements.append(ptext)
    for i in range(len(eventNames)):
        ptext = Paragraph(' * ' + eventNames[i], styles["Normal"])
        elements.append(ptext)

    elements.append(Spacer(1,0.2*inch))
    
    chart = line_chart_with_legend()
    elements.append(chart)
    
#     ptext = Paragraph('Text after the chart', styles["Normal"])
#     elements.append(ptext)
    elements.append(Spacer(1,0.2*inch))
    
    doc.build(elements, onFirstPage=myFirstPage)
    
if __name__ == '__main__':
    main()
    
print(json.dumps( { 'x' : x, 'y' : y } ))




