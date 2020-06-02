import csv

import requests

from record import Record

DATAFILE = './sample_data.csv'

API_URL = 'http://localhost:5000/'

def insert_skills():
    '''
        Post skills to the API
    '''
    url = API_URL + "skill"

    skills = [
        "ALS", "Transport", "Mechanical Ventilation", "Advanced Ventilation",
        "Neuro", "Pulmonary Catheter", "Day 1", "Day 0", "Pacing", "CCRT", "IABP"
    ]

    count = len(skills)
    success = 0
    for s in skills:
        # Make a POST requests to API_URL
        response = requests.post(url, json={"name" : s})
        if response.status_code == 200:
            success += 1
        else:
            print "Got HTTP code", response.status_code, " for ", record

    print success, "/", count, "records inserted."

def insert_designations():
    '''
        Post designations to the API
    '''
    url = API_URL + "designation"

    designations = [
        "NUM", "CNM", "CNE", "CNC", "ANUM", "CNS", "CCRN", "Data Nurse - B0014",
        "Donation Specialist Nursing Coordinator", "MERN", "Team Leader - Group 1",
        "Team Leader - Support", "GradCert CTS", "RN Div 1", "Research Nurse (Y7891)",
        "Transition Program 2016", "Bridging ED", "GCCP 18 Month 2017",
        "GCCP 2017", "Intro2 FEB 2017", "Bridging Sep 16", "Intro to ICU - Sept 2016",
        "Grad Cert Feb 2016 (2 years)"
    ]

    count = len(designations)
    success = 0
    for d in designations:
        # Make a POST requests to API_URL
        response = requests.post(url, json={"name" : d})
        if response.status_code == 200:
            success += 1
        else:
            print "Got HTTP code", response.status_code, " for ", record

    print success, "/", count, "records inserted."

def insert_staff():
    '''
        Parses data file and makes requests to staff API
    '''
    url = API_URL + "staff"

    fieldnames = [
        'last_name',
        'first_name',
        'rosteron_id',
        'type',
        'als',
        'transport',
        'mec_vent',
        'adv_vent',
        'neuro',
        'pul_cat',
        'day1',
        'day0',
        'pacing',
        'ccrt',
        'iabp',
    ]
    with open(DATAFILE, 'r') as csvfile:
        reader = csv.DictReader(csvfile,fieldnames)
        count = 0
        success = 0
        for row in reader:
            count += 1
            record = Record(row)
            # Make a POST requests to API_URL
            response = requests.post(url, json=record.data)
            if response.status_code == 200:
                success += 1
                print count, ". Inserted ", record.data["name"]
            else:
                print "Got HTTP code", response.status_code, " for ", record

        print success, "/", count, "records inserted."

def main():
    insert_designations()
    insert_skills()
    insert_staff()

if __name__ == "__main__":
    main()
