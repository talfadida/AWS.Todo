import shlex #for tokenize the cmd string 
from models import *

class argumentParser(object):
    def __init__(self, line):
        splitter = shlex.shlex(line, posix=True)
        splitter.whitespace_split = True
        args = list(splitter)        
         
        if(args.__len__() <2 or args.__len__() >4):
            raise Exception("inccorect input")
        if(args[0] != "todo"):
            raise Exception("unknown command " + args[0])   
        
        
        if(args.__len__() == 2):
            args.append("") #append fake NewTitle 
        if(args.__len__() == 3):
            args.append("") #append fake NewTitle 

        self.matches = args
        self.req = self.assign_request()

    def assign_request(self):
        switcher = {
            "add-task":     todoRequest(self.matches[2], "", False, 0),
            "update-task":  todoRequest(self.matches[2], self.matches[3], False, 6),
            "complete-task": todoRequest(self.matches[2], "", True, 1),
            "undo-task":    todoRequest(self.matches[2], "", False, 2),
            "delete-task":  todoRequest(self.matches[2], "", False, 3),
            "list-tasks":   todoRequest("", "", False, 4),
            "list-completed-tasks": todoRequest("", "", False, 5)
        }
        func = switcher.get(self.matches[1])
        if func is None:
            raise Exception('commmand is wrong')
        return func
