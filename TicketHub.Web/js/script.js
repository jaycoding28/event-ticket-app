document.addEventListener('DOMContentLoaded', function() {
    const ticketForm = document.getElementById('ticketForm');
    const quantitySelect = document.getElementById('quantity');
    const totalAmount = document.getElementById('totalAmount');
    const confirmation = document.getElementById('confirmation');
    const closeBtn = document.querySelector('.close-btn');
    const closeButton = document.querySelector('.btn-close');
    const loading = document.getElementById('loading');
    
    // Constants
    const TICKET_PRICE = 89.99;
    const API_URL = 'https://eventticketapi-jay.azurewebsites.net/TicketOrder'; 
    
    // Update total amount when quantity changes
    quantitySelect.addEventListener('change', function() {
        const quantity = parseInt(this.value);
        totalAmount.textContent = `$${(quantity * TICKET_PRICE).toFixed(2)}`;
    });
    
    // Form submission
    ticketForm.addEventListener('submit', function(e) {
        e.preventDefault();
        
        // Get form data
        const name = document.getElementById('name').value;
        const email = document.getElementById('email').value;
        const quantity = parseInt(document.getElementById('quantity').value);
        
        // Display loading spinner
        loading.classList.remove('hidden');
        
        // Create order object
        const order = {
            name: name,
            email: email,
            concert: "Sweetener World Tour",
            quantity: quantity,
            purchaseDate: new Date().toISOString()
        };
        
        // Send POST request to API
        fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(order)
        })
        .then(response => {
            loading.classList.add('hidden');
            
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Success:', data);
            
            // Update confirmation modal
            document.getElementById('conf-name').textContent = `Name: ${name}`;
            document.getElementById('conf-email').textContent = `Email: ${email}`;
            document.getElementById('conf-tickets').textContent = `Tickets: ${quantity}`;
            document.getElementById('conf-date').textContent = `Purchase Date: ${new Date().toLocaleString()}`;
            
            // Show confirmation modal
            confirmation.classList.remove('hidden');
            
            // Reset form
            ticketForm.reset();
            totalAmount.textContent = `$${TICKET_PRICE.toFixed(2)}`;
        })
        .catch(error => {
            loading.classList.add('hidden');
            console.error('Error:', error);
            alert('There was an error processing your order. Please try again.');
        });
    });
    
    // Close confirmation modal
    closeBtn.addEventListener('click', function() {
        confirmation.classList.add('hidden');
    });
    
    closeButton.addEventListener('click', function() {
        confirmation.classList.add('hidden');
    });
    
    // Also close when clicking outside the modal
    confirmation.addEventListener('click', function(e) {
        if (e.target === confirmation) {
            confirmation.classList.add('hidden');
        }
    });
});