@GMailAbout
Feature: About GMail
	In order to provide details about gmail
	As google
	I want to welcome users to gmail

Background: 
	Given English About page is loaded

@Ignore
Scenario: Show Gherkin syntax in an ignored test
	Given Some arrangement of the system
	 When Some actions are performed	  
	 Then Some assertion is made


Scenario: English About Page Talks About Security	
	
	 When I go to `Features`
	  And I read security information
	 Then HTTPS Security is mentioned 
	

Scenario: English About Page Talks Links To Google Play			

	 When I go to `For Mobile`
	  And I Navigate to Google Play
	 Then GMail is mentioned



	 