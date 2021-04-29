import uuid
import json

class task(object):
    
    def __init__(self, payload):
        self.task = payload
               
    def asRow(self):
         row_in_table = []
         row_in_table.append(self.task.get('title'))
         row_in_table.append(self.task.get('isCompleted'))
         return row_in_table

class todoRequest(object):
    def __init__(self, Title:str, NewTitle:str, IsCompleted:bool,  Operation: int):
        self.Title = Title
        self.NewTitle = NewTitle
        self.IsCompleted = IsCompleted
        self.OperationUUID = str(uuid.uuid4())       
        self.Operation = Operation

class todoResponse(object):    
    def __init__(self, payload):
        self.__dict__ = json.loads(payload)
