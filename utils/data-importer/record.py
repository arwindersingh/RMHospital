class Record(object):

    def __init__(self, row):
        '''
            Reads a CSV row and inits the class
        '''
        self.data = {}
        self.data["name"] = row['first_name'] +" "+ row['last_name']
        self.data["staff_type"] = row['type']
        self.data["rosteron_id"] = row['rosteron_id']
        self.data["skills"] = []
        if row['als']       == 'TRUE': self.data["skills"].append('ALS')
        if row['transport'] == 'TRUE': self.data["skills"].append("Transport")
        if row['mec_vent']  == 'TRUE': self.data["skills"].append("Mechanical Ventilation")
        if row['adv_vent']  == 'TRUE': self.data["skills"].append("Advanced Ventilation")
        if row['neuro']     == 'TRUE': self.data["skills"].append("Neuro")
        if row['pul_cat']   == 'TRUE': self.data["skills"].append("Pulmonary Catheter")
        if row['day1']      == 'TRUE': self.data["skills"].append("Day 1")
        if row['day0']      == 'TRUE': self.data["skills"].append("Day 0")
        if row['pacing']    == 'TRUE': self.data["skills"].append("Pacing")
        if row['ccrt']      == 'TRUE': self.data["skills"].append("CCRT")
        if row['iabp']      == 'TRUE': self.data["skills"].append("IABP")
    def __str__(self):
        return self.data.__str__()
