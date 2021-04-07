#!/usr/bin/env python
# coding: utf-8

# In[1]:


import sys
import json
import collections
import pandas as pd
from reportlab.platypus import Image
from types import SimpleNamespace
import matplotlib.pyplot as plt

# load input arguments from the text file
filename = sys.argv[ 1 ]
# filename = "C:/Users/barne/GitHubRepos/StrategyBuilder.BFF/Scripts/e0c4b1ed-f783-4fa6-8591-b5f094002a28.txt"
data = ''
with open( filename ) as data_file: 
    data = data_file.read()
    input_args = json.loads( data )

# read value from input args
filename, strategyname, strategydescription, symbolList, eventNames, executedon, executefrom, executeto, x, y = [ input_args.get( key ) for key in [ 'filename', 'strategyname', 'strategydescription', 'symbolList', 'eventNames', 'executedon', 'executefrom',  'executeto',  'x',  'y' ] ]

input_args = json.loads(data, object_hook=lambda d: SimpleNamespace(**d))


# input_args.accountPerformance
# loaddata = json.loads( data )
# df = pd.json_normalize(loaddata['accountPerformance'])
#df.set_index('Date')
# df.index = pd.to_datetime(df["Date"])
# df = df.drop(columns=["Date"])
# df.info()
# for i, col in enumerate(df.columns):
#     df[col].plot()

# plt.title('Price Evolution Comparison')

# plt.xticks(rotation=70)
# plt.legend(df.columns)
# plt.savefig(input_args.imagefilename, bbox_inches='tight')

    
print(input_args.imagefilename)

# In[ ]:




