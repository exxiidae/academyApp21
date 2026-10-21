async function loadStudents() {
    const errorEl = document.getElementById("error");
    const body = document.getElementById("studentsBody");
    try {
        const res = await fetch("/students/all");
        if (!res.ok) throw new Error("HTTP " + res.status);
        const students = await res.json();

        body.innerHTML = "";
        for (const s of students) {
            body.innerHTML += `
                <tr>
                    <td>${s.studentId}</td>
                    <td>${s.firstName}</td>
                    <td>${s.lastName}</td>
                    <td>${s.age}</td>
                    <td>${s.groupId}</td>
                    <td>
                        <button onclick="startEdit(${s.studentId})" style="cursor:pointer; background:#f39c12; color:white; border:none; padding:4px 8px; border-radius:4px;">Ред.</button>
                        <button onclick="deleteStudent(${s.studentId})" style="cursor:pointer; background:#e74c3c; color:white; border:none; padding:4px 8px; border-radius:4px;">Удал.</button>
                    </td>
                </tr>`;
        }
        errorEl.textContent = "";
    } catch (err) {
        errorEl.textContent = "Ошибка загрузки студентов: " + err.message;
    }
}

async function loadGroups() {
    const errorEl = document.getElementById("error");
    const body = document.getElementById("groupsBody");
    try {
        const res = await fetch("/groups/all");
        if (!res.ok) throw new Error("HTTP " + res.status);
        const groups = await res.json();

        body.innerHTML = "";
        for (const g of groups) {
            body.innerHTML += `
                <tr>
                    <td>${g.groupId}</td>
                    <td>${g.groupName}</td>
                </tr>`;
        }
    } catch (err) {
        errorEl.textContent = "Ошибка загрузки групп: " + err.message;
    }
}

async function loadStudentById() {
    const id = document.getElementById("searchId").value;
    const result = document.getElementById("studentResult");

    if (!id) { result.textContent = "Введите ID!"; return; }

    try {
        const res = await fetch(`/students/${id}`);
        if (res.status === 404) { result.textContent = `❌ Студент с ID=${id} не найден`; return; }
        if (!res.ok) throw new Error("HTTP " + res.status);

        const s = await res.json();
        result.innerHTML = `
            <strong>✅ Найден студент:</strong><br>
            ID: ${s.studentId}<br> Имя: ${s.firstName}<br> Фамилия: ${s.lastName}<br> 
            Возраст: ${s.age}<br> GroupId: ${s.groupId}
        `;
    } catch (err) {
        result.textContent = "Ошибка: " + err.message;
    }
}

async function addStudent() {
    const resultEl = document.getElementById("addResult");
    
    const student = {
        firstName: document.getElementById("addFirstName").value.trim(),
        lastName: document.getElementById("addLastName").value.trim(),
        age: parseInt(document.getElementById("addAge").value) || 0,
        groupId: parseInt(document.getElementById("addGroupId").value) || 0
    };

    if (!student.firstName || !student.lastName) {
        resultEl.textContent = "Имя и фамилия обязательны!";
        return;
    }

    try {
        const res = await fetch("/students/add", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(student)
        });

        if (!res.ok) {
            const errData = await res.json();
            throw new Error(errData.message || "HTTP " + res.status);
        }

        resultEl.innerHTML = `Студент добавлен!`;
        document.getElementById("addFirstName").value = "";
        document.getElementById("addLastName").value = "";
        document.getElementById("addAge").value = "";
        document.getElementById("addGroupId").value = "";
        loadStudents();
    } catch (err) {
        resultEl.textContent = "Ошибка: " + err.message;
    }
}

// --- НОВОЕ ДЛЯ 18 УРОКА ---

async function deleteStudent(id) {
    if (!confirm(`Вы уверены, что хотите удалить студента с ID ${id}?`)) return;

    try {
        const res = await fetch(`/students/${id}`, { method: "DELETE" });
        if (!res.ok) throw new Error("HTTP " + res.status);
        loadStudents();
    } catch (err) {
        alert("Ошибка удаления: " + err.message);
    }
}

async function startEdit(id) {
    try {
        const res = await fetch(`/students/${id}`);
        if (!res.ok) throw new Error("Студент не найден");
        const s = await res.json();

        document.getElementById("editForm").style.display = "block";
        document.getElementById("editIdLabel").textContent = `#${s.studentId}`;
        document.getElementById("editId").value = s.studentId;
        document.getElementById("editFirstName").value = s.firstName;
        document.getElementById("editLastName").value = s.lastName;
        document.getElementById("editAge").value = s.age;
        document.getElementById("editGroupId").value = s.groupId;
        document.getElementById("editResult").textContent = "";
        
        window.scrollTo({ top: document.getElementById("editForm").offsetTop, behavior: 'smooth' });
    } catch (err) {
        alert(err.message);
    }
}

function cancelEdit() {
    document.getElementById("editForm").style.display = "none";
}

async function updateStudent() {
    const id = document.getElementById("editId").value;
    const resultEl = document.getElementById("editResult");
    
    const student = {
        firstName: document.getElementById("editFirstName").value.trim(),
        lastName: document.getElementById("editLastName").value.trim(),
        age: parseInt(document.getElementById("editAge").value) || 0,
        groupId: parseInt(document.getElementById("editGroupId").value) || 0
    };

    try {
        const res = await fetch(`/students/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(student)
        });

        if (!res.ok) throw new Error("HTTP " + res.status);

        resultEl.textContent = "Успешно обновлено!";
        loadStudents();
        setTimeout(cancelEdit, 1000);
    } catch (err) {
        resultEl.textContent = "Ошибка: " + err.message;
    }
}

loadStudents();
loadGroups();