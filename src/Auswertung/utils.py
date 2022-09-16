import re

from matplotlib import pyplot as plt


def rename_ids(row):
    tmp = row.split('/')[-1]
    tmp = re.sub(r'-r1|-r2|-r3', '', tmp)
    tmp = re.sub(r'-application-inishgts', '', tmp)
    tmp = re.sub(r'-ai', '', tmp)
    tmp = re.sub(r'-linux', '', tmp)
    tmp = re.sub(r'-fn', '-function', tmp)
    tmp = re.sub(r'low-', 'service-bus-low-', tmp)
    tmp = re.sub(r'mid-', 'service-bus-mid-', tmp)
    tmp = re.sub(r'high-', 'service-bus-high-', tmp)
    tmp = re.sub(r'load-2-grpc', 'grpc-low', tmp)
    tmp = re.sub(r'load-25-grpc', 'grpc-mid', tmp)
    tmp = re.sub(r'load-250-grpc', 'grpc-high', tmp)
    tmp = re.sub(r'load-2-', 'web-api-low-', tmp)
    tmp = re.sub(r'load-25-', 'web-api-mid-', tmp)
    tmp = re.sub(r'load-250-', 'web-api-high-', tmp)
    return tmp




def get_scenario_type(name):
    if "load" in name or "reports" in name:
        return "request-reply"
    if "low" in name or "mid" in name or "high" in name or "min" in name:
        return "messaging"


def get_scenario_communication(name, scenario_type):
    if scenario_type == "request-reply":
        if "grpc" in name:
            return "gRPC"
        return "Web-API"
    return "Service Bus"


def get_scenario_load_level(name, scenario_type):
    if scenario_type == "messaging":
        if "low" in name:
            return "low"
        if "mid" in name:
            return "mid"
        if "high" in name:
            return "high"
        if "min1" in name:
            return "min1"
        if "min8" in name:
            return "min8"
        if "min20" in name:
            return "min20"
    if scenario_type == "request-reply":
        if "2-" in name:
            return "low"
        if "25-" in name:
            return "mid"
        if "250-" in name:
            return "high"


def get_hosting_environment_type(name):
    if "vm" in name:
        return "VM"
    if "appservice" in name:
        return "App Service"
    if "docker" in name:
        return "Container Instance"
    if "function" in name:
        return "Function"


def get_os(name):
    if name == "reports-2-appservice":
        return "Windows"
    if name == "reports-25-appservice":
        return "Windows"
    if name == "reports-250-appservice":
        return "Windows"
    return "Linux"


def format_load_level(old):
    if old == "low":
        return "Niedrige Last"
    if old == "mid":
        return "Mittlere Last"
    if old == "high":
        return "Hohe Last"


def load_level_to_numeric(old):
    if old == "low":
        return 0
    if old == "mid":
        return 1
    if old == "high":
        return 2
