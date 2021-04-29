import requests

from models import *
from utils import *
from tabulate import tabulate


url = "https://y0ygimjuu3.execute-api.us-east-2.amazonaws.com/Prod/api/operation"
 
while(True):
    try:         
        cmdArgs = argumentParser(input('todo> '))       
        rawResp = requests.post(url, json= cmdArgs.req.__dict__ , headers= {"Content-Type": "application/json"} )
        todoResp = todoResponse(rawResp.text)
        if not todoResp.isSuccess:
            print('Error: ' + todoResp.errorMessage)
        if todoResp.isSuccess and (todoResp.operation == 4 or  todoResp.operation == 5):
            table = []
            [table.append(task(item).asRow()) for item in todoResp.taskList]             
            print(tabulate(table, headers=['NAME', 'COMPLETED']))
             
    except Exception as x:
        print(x)     
    
