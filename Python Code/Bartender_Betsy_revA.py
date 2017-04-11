import RPi.GPIO as GPIO
import time
import threading

# --------------------------------------------------------------------------
# Bartender Betsy Control Code
# --------------------------------------------------------------------------

# Output message to let user know program has started

print ("\nWelcome to Bartender Betsy!")
print ("\nInitializing program...")

# All pins are set to use the broadcom numbering scheme on the RPi, refer to
# this wiring standard when setting up or changing pins

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

# Define the GPIO pins for the pumps

p1 = 17		# Pin 11 (GPIO17) 
p2 = 18		# Pin 12 (GPIO18)
p3 = 27		# Pin 13 (GPIO27)
p4 = 22		# Pin 15 (GPIO22)
p5 = 23		# Pin 16 (GPIO23)
p6 = 10		# Pin 19 (GPIO10)
p7 = 9		# Pin 21 (GPIO09)
p8 = 25		# Pin 22 (GPIO25)
p9 = 11		# Pin 23 (GPIO11)
p10 = 8		# Pin 24 (GPIO08)
p11 = 7		# Pin 26 (GPIO07)		
p12 = 5		# Pin 29 (GPIO05)
p13 = 6		# Pin 31 (GPIO06)
p14 = 12	# Pin 32 (GPIO12)
p15 = 13	# Pin 33 (GPIO13)
p16 = 19	# Pin 35 (GPIO19)

# Define other variables needed for control code
pour_duration = 1 		# Duration in seconds needed for 1 oz of liquid pour

# Setup all pins as output

GPIO.setup(p1,GPIO.OUT)
GPIO.setup(p2,GPIO.OUT)
GPIO.setup(p3,GPIO.OUT)
GPIO.setup(p4,GPIO.OUT)
GPIO.setup(p5,GPIO.OUT)
GPIO.setup(p6,GPIO.OUT)
GPIO.setup(p7,GPIO.OUT)
GPIO.setup(p8,GPIO.OUT)
GPIO.setup(p9,GPIO.OUT)
GPIO.setup(p10,GPIO.OUT)
GPIO.setup(p11,GPIO.OUT)
GPIO.setup(p12,GPIO.OUT)
GPIO.setup(p13,GPIO.OUT)
GPIO.setup(p14,GPIO.OUT)
GPIO.setup(p15,GPIO.OUT)
GPIO.setup(p16,GPIO.OUT)

# Set all pin values to low to start to prevent actuation upon startup

GPIO.output(p1,False)
GPIO.output(p2,False)
GPIO.output(p3,False)
GPIO.output(p4,False)
GPIO.output(p5,False)
GPIO.output(p6,False)
GPIO.output(p7,False)
GPIO.output(p8,False)
GPIO.output(p9,False)
GPIO.output(p10,False)
GPIO.output(p11,False)
GPIO.output(p12,False)
GPIO.output(p13,False)
GPIO.output(p14,False)
GPIO.output(p15,False)
GPIO.output(p16,False)

# Setup of pins complete

print ("\nAwaiting drink requests...")

# Function definitions

def all_pumps_on():
	GPIO.output(p1,True)
	GPIO.output(p2,True)
	GPIO.output(p3,True)
	GPIO.output(p4,True)
	GPIO.output(p5,True)
	GPIO.output(p6,True)
	GPIO.output(p7,True)
	GPIO.output(p8,True)
	GPIO.output(p9,True)
	GPIO.output(p10,True)
	GPIO.output(p11,True)
	GPIO.output(p12,True)
	GPIO.output(p13,True)
	GPIO.output(p14,True)
	GPIO.output(p15,True)
	GPIO.output(p16,True)
	print("\nAll pumps on.")

def all_pumps_off():
	GPIO.output(p1,False)
	GPIO.output(p2,False)
	GPIO.output(p3,False)
	GPIO.output(p4,False)
	GPIO.output(p5,False)
	GPIO.output(p6,False)
	GPIO.output(p7,False)
	GPIO.output(p8,False)
	GPIO.output(p9,False)
	GPIO.output(p10,False)
	GPIO.output(p11,False)
	GPIO.output(p12,False)
	GPIO.output(p13,False)
	GPIO.output(p14,False)
	GPIO.output(p15,False)
	GPIO.output(p16,False)
	print("\nAll pumps off.")

def pump_toggle(ID, status):
	if status == 1:
		try:
			GPIO.output(ID,True)
			print ("Pump " & ID & " on.")
		except Exception, e:
			raise e
	elif status == 0:
		try:
			GPIO.output(ID,False)
			print ("Pump " & ID & " off.")
		except Exception, e:
			raise e

def pump_actuate(ID,duration):
	try:
		GPIO.output(ID,True)
		time.sleep(duration)
		GPIO.output(ID,False)
	except Exception, e:
		raise e

# Sample of how I think recipe creation would go

# User selects recipe, using pumps 1,2,3
if ing_1 in recipe: 	# Check to see if ingridient 1 is used in the recipe list
	ID_1 = 'p1' 	# Assign ID of pump (p1-p16)
	duration_1 = amount * pour_duration 	# Calculate the duration of the pour
	ing_1_trd = threading.Thread(target=pump_actuate(ID_1,duration_1)) 	# Begin new thread
# Repeat steps for all ingridients in recipe list
elif ing_2 in recipe:
	ID_2 = 'p2'
	duration_2 = amount * pour_duration
	ing_2_trd = threading.Thread(target=pump_actuate(ID_2,duration_2))
elif ing_3 in recipe:
	ID_3 = 'p3'
	duration_3 = amount * pour_duration
	ing_3_trd = threading.Thread(target=pump_actuate(ID_3,duration_3))
else
	# Other ingridient threads would cover all of the pumps













