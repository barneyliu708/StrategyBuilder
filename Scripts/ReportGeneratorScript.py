#!/usr/bin/env python
# coding: utf-8

# In[44]:




import sys
import json

# load input arguments from the text file
filename = sys.argv[ 1 ]
#filename = "C:/Users/barne/OneDrive/Documents/CPT/HU/Semester 5/GRAD 695/Project/e6b87950-e68c-49e7-96ea-aef0898ebf4c.txt"
with open( filename ) as data_file:   
    input_args = json.loads( data_file.read() )
# input_args = json.loads( "{ ""x"" : [-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5], ""y"" : [-0.0021784544551539569, -0.012620231871447715, -0.032682647406455115, -0.015832210571565322, -0.010029744912551973, 0.0, 0.012052718214074077, 0.0091328708527324363, 0.025531059757424972, 0.028779609442359477, 0.02639368866762375] }" )

# read value from input args
filename, strategyname, strategydescription, symbol, eventNames, executedon, executefrom, executeto, x, y = [ input_args.get( key ) for key in [ 'filename', 'strategyname', 'strategydescription', 'symbol', 'eventNames', 'executedon', 'executefrom', 'executeto', 'x', 'y' ] ]

# print(json.dumps( { 'x' : x, 'y' : y } ))

chartdata = []
for i in range(len(x)):
    chartdata.append((x[i], y[i]))

# print(json.dumps( { 'sum' : x + y , 'subtract' : x - y } )) 
# print( chartdata )

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
#     data = [[(19010706, 3.3900000000000001), (19010806, 3.29), (19010906, 3.2999999999999998), (19011006, 3.29), (19011106, 3.3399999999999999), (19011206, 3.4100000000000001), (19020107, 3.3700000000000001), (19020207, 3.3700000000000001), (19020307, 3.3700000000000001), (19020407, 3.5), (19020507, 3.6200000000000001), (19020607, 3.46), (19020707, 3.3900000000000001)]]
    drawing = Drawing(width=400, height=200)
    my_title = String(50, 120, 'Event Impact on Stock Price Change', fontSize=11)
    chart = LinePlot()
#     chart.sideLabels = True
#     line.x = 150
#     line.y = 65
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
#     self.legend.dxTextSpace    = 7
#     self.legend.boxAnchor      = 'nw'
#     self.legend.subCols.dx        = 0
#     self.legend.subCols.dy        = -2
#     self.legend.subCols.rpad      = 0
#     self.legend.columnMaximum  = 1
#     self.legend.deltax         = 1
#     self.legend.deltay         = 0
#     self.legend.dy             = 5
#     self.legend.y              = 135
#     self.legend.x              = 120
    chart.lines.symbol.kind        = 'FilledCross'
    chart.lines.symbol.size        = 5
    chart.lines.symbol.angle       = 45
    
#     pie.labels = [letter for letter in 'abcdefg']
#     pie.slices.strokeWidth = 0.5
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
    
# from reportlab.lib.colors import purple, PCMYKColor, black, pink, green, blue
# from reportlab.graphics.charts.lineplots import LinePlot
# from reportlab.graphics.charts.legends import LineLegend
# from reportlab.graphics.shapes import Drawing, _DrawingEditorMixin
# from reportlab.lib.validators import Auto
# from reportlab.graphics.widgets.markers import makeMarker
# from reportlab.pdfbase.pdfmetrics import stringWidth, EmbeddedType1Face, registerTypeFace, Font, registerFont
# from reportlab.graphics.charts.axes import XValueAxis, YValueAxis, AdjYValueAxis, NormalDateXValueAxis

# class SmileyMarkerChart(_DrawingEditorMixin,Drawing):
#     '''
#     Chart Features
#     ==============

#     This chart is closely related to the 'silly' line chart
#     with markers. The key attributes that have changed are:

#     - **chart.lines.symbol.kind** was changed from 'FilledCross' to 'Smiley'
#     - **chart.lines.symbol.size** was changed from 5 to 15
#     - **chart.lines.symbol.angle** was changed from 45 to 0 in order
#     to prevent the smiley faces from being rotated
#     - **legend.columnMaximum** was changed from 1 to 2, forcing the 
#     legend into a single stacked column
#     - **legend.y** was increased to push the legend up
#     - **legend.x** was increased to move it to the right across the chart
#     '''
#     def __init__(self,width=258,height=150,*args,**kw):
#         Drawing.__init__(self,width,height,*args,**kw)
#         # font
#         fontName = 'Helvetica'
#         fontSize = 7
#         # chart
#         self._add(self,LinePlot(),name='chart',validate=None,desc=None)
#         self.chart.y                = 16
#         self.chart.x                = 32
#         self.chart.width            = 212
#         self.chart.height           = 90
#         # line styles
#         self.chart.lines.strokeWidth     = 0
#         self.chart.lines.symbol= makeMarker('FilledSquare')
#         # x axis
#         self.chart.xValueAxis = NormalDateXValueAxis()
#         self.chart.xValueAxis.labels.fontName          = fontName
#         self.chart.xValueAxis.labels.fontSize          = fontSize-1
#         self.chart.xValueAxis.forceEndDate             = 1
#         self.chart.xValueAxis.forceFirstDate           = 1
#         self.chart.xValueAxis.labels.boxAnchor      ='autox'
#         self.chart.xValueAxis.xLabelFormat          = '{d}-{MMM}'
#         self.chart.xValueAxis.maximumTicks          = 5
#         self.chart.xValueAxis.minimumTickSpacing    = 0.5
#         self.chart.xValueAxis.niceMonth             = 0
#         self.chart.xValueAxis.strokeWidth           = 1
#         self.chart.xValueAxis.loLLen                = 5
#         self.chart.xValueAxis.hiLLen                = 5
#         self.chart.xValueAxis.gridEnd               = self.width
#         self.chart.xValueAxis.gridStart             = self.chart.x-10
#         # y axis
#         #self.chart.yValueAxis = AdjYValueAxis()
#         self.chart.yValueAxis.visibleGrid           = 1
#         self.chart.yValueAxis.visibleAxis=0
#         self.chart.yValueAxis.labels.fontName       = fontName
#         self.chart.yValueAxis.labels.fontSize       = fontSize -1
#         self.chart.yValueAxis.labelTextFormat       = '%0.2f%%'
#         self.chart.yValueAxis.strokeWidth           = 0.25
#         self.chart.yValueAxis.visible               = 1
#         self.chart.yValueAxis.labels.rightPadding   = 5
#         #self.chart.yValueAxis.maximumTicks          = 6
#         self.chart.yValueAxis.rangeRound            ='both'
#         self.chart.yValueAxis.tickLeft              = 7.5
#         self.chart.yValueAxis.minimumTickSpacing    = 0.5
#         self.chart.yValueAxis.maximumTicks          = 8
#         self.chart.yValueAxis.forceZero             = 0
#         self.chart.yValueAxis.avoidBoundFrac = 0.1
#         # legend
#         self._add(self,LineLegend(),name='legend',validate=None,desc=None)
#         self.legend.fontName         = fontName
#         self.legend.fontSize         = fontSize
#         self.legend.alignment        ='right'
#         self.legend.dx           = 5
#         # sample data
#         self.chart.data = [[(19010706, 3.3900000000000001), (19010806, 3.29), (19010906, 3.2999999999999998), (19011006, 3.29), (19011106, 3.3399999999999999), (19011206, 3.4100000000000001), (19020107, 3.3700000000000001), (19020207, 3.3700000000000001), (19020307, 3.3700000000000001), (19020407, 3.5), (19020507, 3.6200000000000001), (19020607, 3.46), (19020707, 3.3900000000000001)], [(19010706, 3.2000000000000002), (19010806, 3.1200000000000001), (19010906, 3.1400000000000001), (19011006, 3.1400000000000001), (19011106, 3.1699999999999999), (19011206, 3.23), (19020107, 3.1899999999999999), (19020207, 3.2000000000000002), (19020307, 3.1899999999999999), (19020407, 3.3100000000000001), (19020507, 3.4300000000000002), (19020607, 3.29), (19020707, 3.2200000000000002)]]
#         self.chart.lines[0].strokeColor = PCMYKColor(0,100,100,40,alpha=100)
#         self.chart.lines[1].strokeColor = PCMYKColor(100,0,90,50,alpha=100)
#         self.chart.xValueAxis.strokeColor             = PCMYKColor(100,60,0,50,alpha=100)
#         self.legend.colorNamePairs = [(PCMYKColor(0,100,100,40,alpha=100), 'Bovis Homes'), (PCMYKColor(100,0,90,50,alpha=100), 'HSBC Holdings')]
#         self.chart.lines.symbol.x           = 0
#         self.chart.lines.symbol.strokeWidth = 0
#         self.chart.lines.symbol.arrowBarbDx = 5
#         self.chart.lines.symbol.strokeColor = PCMYKColor(0,0,0,0,alpha=100)
#         self.chart.lines.symbol.fillColor   = None
#         self.chart.lines.symbol.arrowHeight = 5
#         self.legend.dxTextSpace    = 7
#         self.legend.boxAnchor      = 'nw'
#         self.legend.subCols.dx        = 0
#         self.legend.subCols.dy        = -2
#         self.legend.subCols.rpad      = 0
#         self.legend.columnMaximum  = 1
#         self.legend.deltax         = 1
#         self.legend.deltay         = 0
#         self.legend.dy             = 5
#         self.legend.y              = 135
#         self.legend.x              = 120
#         self.chart.lines.symbol.kind        = 'FilledCross'
#         self.chart.lines.symbol.size        = 5
#         self.chart.lines.symbol.angle       = 45

# if __name__=="__main__": #NORUNTESTS
#     SmileyMarkerChart().save(formats=['pdf'], outDir='./test',fnRoot=None)

print(json.dumps( { 'x' : x, 'y' : y } ))


# In[ ]:





# In[ ]:




