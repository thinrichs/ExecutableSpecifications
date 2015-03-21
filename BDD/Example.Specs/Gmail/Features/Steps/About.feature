@GMailAbout
Feature: About GMail
	In order to provide details about gmail
	As google
	I want to welcome users to gmail

Background: 
	Given English About page is loaded

Scenario: English About Page Talks About Security	
	
	 When I go to `Features`
	  And I read security information
	 Then HTTPS Security is mentioned 
	

Scenario: English About Page Talks about Google Play			

	 When I go to `For Mobile`
	  And Nvigate to Google Play
	 Then GMail is mentioned
	 