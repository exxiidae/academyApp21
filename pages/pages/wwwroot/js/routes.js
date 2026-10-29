async function loadStudents() {
    const body = document.getElementById("studentsBody");
    try {
        const res = await fetch("/students/all");
        const students = await res.json();
        body.innerHTML = "";
        for (const s of students) {
            body.innerHTML += `<tr><td>${s.studentId}</td><td>${s.firstName}</td><td>${s.lastName}</td><td>${s.age}</td><td>${s.groupId}</td></tr>`;
        }
    } catch (err) { console.error(err); }
}

async function loadTeachers() {
    const body = document.getElementById("teachersBody");
    try {
        const res = await fetch("/teachers/all");
        const teachers = await res.json();
        body.innerHTML = "";
        for (const t of teachers) {
            body.innerHTML += `<tr><td>${t.teacherId}</td><td>${t.firstName}</td><td>${t.lastName}</td></tr>`;
        }
    } catch (err) { console.error(err); }
}

async function loadGroups() {
    const body = document.getElementById("groupsBody");
    try {
        const res = await fetch("/groups/all");
        const groups = await res.json();
        body.innerHTML = "";
        for (const g of groups) {
            body.innerHTML += `<tr><td>${g.groupId}</td><td>${g.groupName}</td></tr>`;
        }
    } catch (err) { console.error(err); }
}

function toggleAddFields() {
    const type = document.getElementById("addType").value;
    const studentFields = document.getElementById("studentFields");
    if (type === "student") {
        studentFields.style.display = "block";
    } else {
        studentFields.style.display = "none";
    }
}

async function addEntity() {
    const type = document.getElementById("addType").value;
    const resultEl = document.getElementById("addResult");
    
    let data = {
        firstName: document.getElementById("addFirstName").value.trim(),
        lastName: document.getElementById("addLastName").value.trim()
    };

    if (!data.firstName || !data.lastName) {
        resultEl.textContent = "Имя и фамилия обязательны!";
        return;
    }

    let url = "";

    if (type === "student") {
        data.age = parseInt(document.getElementById("addAge").value) || 0;
        data.groupId = parseInt(document.getElementById("addGroupId").value) || 0;
        url = "/students/add";
    } else {
        url = "/teachers/add";
    }

    try {
        const res = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        if (!res.ok) throw new Error("Ошибка сервера");

        resultEl.innerHTML = `Успешно добавлено!`;
        
        document.getElementById("addFirstName").value = "";
        document.getElementById("addLastName").value = "";
        document.getElementById("addAge").value = "";
        document.getElementById("addGroupId").value = "";

        if (type === "student") loadStudents();
        else loadTeachers();

    } catch (err) {
        resultEl.textContent = "Ошибка: " + err.message;
    }
}

loadStudents();
loadTeachers();
loadGroups();
toggleAddFields();